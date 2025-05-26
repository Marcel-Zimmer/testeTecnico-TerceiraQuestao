using Microsoft.AspNetCore.Mvc;
using questaoTres.Dtos;
using questaoTres.Services;
using System;
using System.Collections.Generic;

namespace ProductCatalogApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly InMemoryDataService _dataService;

        public CategoriesController(InMemoryDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCategory = _dataService.AddCategory(createCategoryDto);
            if (newCategory == null)
            {
                return Conflict(new { message = $"Categoria '{createCategoryDto.Name}' já existe." });
            }
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoryById(int id)
        {
            var category = _dataService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public IActionResult GetAllCategories()
        {
            return Ok(_dataService.GetAllCategories());
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                if (!_dataService.CategoryExists(id))
                {
                    return NotFound(new { message = "Categoria não encontrada." });
                }
                var success = _dataService.DeleteCategory(id);
                if (success)
                {
                    return NoContent();
                }
                return NotFound(new { message = "Categoria não encontrada para deleção." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}