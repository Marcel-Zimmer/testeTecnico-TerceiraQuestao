const API_BASE_URL = 'http://localhost:5069/api';
const categoryNameInput = document.getElementById('categoryName');
const categoriesList = document.getElementById('categoriesList');
const productNameInput = document.getElementById('productName');
const productCategorySelect = document.getElementById('productCategorySelect');
const productPriceInput = document.getElementById('productPrice');
const productsTableBody = document.getElementById('productsTable').getElementsByTagName('tbody')[0];
const messageArea = document.getElementById('messageArea');

function showMessage(message, type = 'success') {
    messageArea.textContent = message;
    messageArea.className = `message-area ${type}`;
    setTimeout(() => {
        messageArea.textContent = '';
        messageArea.className = 'message-area';
    }, 5000);
}

async function fetchApi(endpoint, options = {}) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
        if (!response.ok) {
            const errorData = await response.json().catch(() => ({ message: `Erro HTTP: ${response.status}` }));
            throw new Error(errorData.message || `Erro ${response.status}`);
        }
        if (response.status === 204) {
            return null;
        }
        return await response.json();
    } catch (error) {
        showMessage(error.message, 'error');
        console.error('API Error:', error);
        throw error;
    }
}

async function loadCategories() {
    try {
        const categories = await fetchApi('/categories');
        categoriesList.innerHTML = '';
        productCategorySelect.innerHTML = '<option value="">Selecione uma Categoria</option>';

        categories.forEach(category => {
            const listItem = document.createElement('li');
            listItem.textContent = `ID: ${category.id} - Nome: ${category.name}`;

            const deleteButton = document.createElement('button');
            deleteButton.textContent = 'Deletar';
            deleteButton.classList.add('delete-btn');
            deleteButton.onclick = () => deleteCategory(category.id);

            listItem.appendChild(deleteButton);
            categoriesList.appendChild(listItem);

            const option = document.createElement('option');
            option.value = category.id;
            option.textContent = category.name;
            productCategorySelect.appendChild(option);
        });
    } catch (error) {
        
    }
}

async function addCategory() {
    const name = categoryNameInput.value.trim();
    if (!name) {
        showMessage('Nome da categoria é obrigatório.', 'error');
        return;
    }
    try {
        await fetchApi('/categories', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name })
        });
        showMessage('Categoria cadastrada com sucesso!');
        categoryNameInput.value = '';
        loadCategories();
    } catch (error) {
        
    }
}

async function deleteCategory(categoryId) {
    if (!confirm('Tem certeza que deseja deletar esta categoria?')) {
        return;
    }
    try {
        await fetchApi(`/categories/${categoryId}`, { method: 'DELETE' });
        showMessage('Categoria deletada com sucesso!');
        loadCategories();
        loadProducts();
    } catch (error) {
        
    }
}

async function addProduct() {
    const name = productNameInput.value.trim();
    const categoryId = productCategorySelect.value;
    const price = parseFloat(productPriceInput.value);

    if (!name || !categoryId || isNaN(price)) {
        showMessage('Todos os campos do produto são obrigatórios e o preço deve ser um número.', 'error');
        return;
    }

    try {
        await fetchApi('/products', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, categoryId: parseInt(categoryId), price })
        });
        showMessage('Produto cadastrado com sucesso!');
        productNameInput.value = '';
        productCategorySelect.value = '';
        productPriceInput.value = '';
        loadProducts();
    } catch (error) {
        
    }
}

async function loadProducts() {
    try {
        const products = await fetchApi('/products');
        productsTableBody.innerHTML = '';
        products.forEach(product => {
            const row = productsTableBody.insertRow();
            row.insertCell().textContent = product.id;
            row.insertCell().textContent = product.name;
            row.insertCell().textContent = product.categoryName;
            row.insertCell().textContent = product.price.toFixed(2);
        });
    } catch (error) {
        
    }
}

document.addEventListener('DOMContentLoaded', () => {
    loadCategories();
    loadProducts();
});