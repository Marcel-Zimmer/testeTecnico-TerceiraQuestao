using Microsoft.AspNetCore.Mvc;
using questaoTres.Dtos;
using questaoTres.Services;
using System.Collections.Generic;

namespace ProductCatalogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly InMemoryDataService _dataService;

        public ProductsController(InMemoryDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateProduct([FromBody] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newProduct = _dataService.AddProduct(createProductDto);
            if (newProduct == null)
            {
                return NotFound(new { message = $"Categoria com ID {createProductDto.CategoryId} não encontrada." });
            }
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDetailDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllProducts()
        {
            return Ok(_dataService.GetAllProducts());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProductById(int id)
        {
            var product = _dataService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}