using questaoTres.Models;
using questaoTres.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace questaoTres.Services
{
    public class InMemoryDataService
    {
        private readonly List<Category> _categories = new List<Category>();
        private readonly List<Product> _products = new List<Product>();
        private int _nextCategoryId = 1;
        private int _nextProductId = 1;

        public IEnumerable<CategoryDto> GetAllCategories()
        {
            return _categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name });
        }

        public CategoryDto GetCategoryById(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            return category == null ? null : new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public CategoryDto AddCategory(CreateCategoryDto createCategoryDto)
        {
            if (_categories.Any(c => c.Name.Equals(createCategoryDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return null;
            }
            var category = new Category
            {
                Id = _nextCategoryId++,
                Name = createCategoryDto.Name
            };
            _categories.Add(category);
            return new CategoryDto { Id = category.Id, Name = category.Name };
        }

        public bool DeleteCategory(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return false;
            }
            if (_products.Any(p => p.CategoryId == id))
            {
                throw new InvalidOperationException("Categoria possui produtos associados.");
            }
            _categories.Remove(category);
            return true;
        }

        public IEnumerable<ProductDetailDto> GetAllProducts()
        {
            return _products.Select(p =>
            {
                var category = _categories.FirstOrDefault(c => c.Id == p.CategoryId);
                return new ProductDetailDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryName = category?.Name ?? "N/A",
                    Price = p.Price
                };
            });
        }

        public ProductDetailDto GetProductById(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return null;
            }
            var category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
            return new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryName = category?.Name ?? "N/A",
                Price = product.Price
            };
        }

        public ProductDto AddProduct(CreateProductDto createProductDto)
        {
            if (!_categories.Any(c => c.Id == createProductDto.CategoryId))
            {
                return null;
            }
            var product = new Product
            {
                Id = _nextProductId++,
                Name = createProductDto.Name,
                CategoryId = createProductDto.CategoryId,
                Price = createProductDto.Price
            };
            _products.Add(product);
            return new ProductDto { Id = product.Id, Name = product.Name, CategoryId = product.CategoryId, Price = product.Price };
        }

        public bool CategoryExists(int id)
        {
            return _categories.Any(c => c.Id == id);
        }
    }
}