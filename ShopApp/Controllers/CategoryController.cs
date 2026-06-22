using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {
        [HttpGet]
        public List<Category> GetCategories()
        {
            return _categoryService.GetAllCategoires();
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById([FromRoute] int id)
        {
            var category = new Category()
            {
                Title = $"Test Category {id}",
            };
            return Ok(category);
        }

        [HttpPost]
        public IActionResult AddNewCategory([FromBody] Category category)
        {
            _categoryService.AddCategory(category);
            return Created();
        }
    }
}
