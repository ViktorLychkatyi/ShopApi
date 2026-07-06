using Microsoft.AspNetCore.Mvc;
using ShopApi.Requests.Categories;
using ShopApi.Services;
using ShopApplication.DTOs;
using ShopApplication.DTOs.Category;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces;
using ShopApplication.Interfaces.Services;
using ShopApplication.Services;
using ShopDomain.Models;
using ShopApi.Interfaces;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController(ICategoryService _categoryService, IImageService _imageService, IConfiguration _configuration) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest dto)
        {
            if (dto.Image != null)
            {
                dto.Url = (await _imageService.SaveFileAsync(dto.Image, _configuration["DirnameForFiles:Categories"])) ?? string.Empty;
            }

            var createDto = new CategoryCreateDTO
            {
                Name = dto.Name,
                Url = dto.Url,
                Slug = dto.Slug,
                ParentId = dto.ParentId == 0 ? null : dto.ParentId,
            };


            var id = await _categoryService.CreateCategoryAsync(createDto);

            return Ok($"Category created {id}");
            //return CreatedAtAction(nameof(GetCategoryById),
            //        // назва методу new { id },              
            //        // параметри маршруту new { id });             
            //        // тіло відповіді
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest dto) //FromBody
        //{
        //    int? id = await _categoryService.CreateCategoryAsync(dto);
        //    return Ok($"Category created {id}");


        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
                return NotFound("Category not found");

            return Ok(category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryById(int id, [FromBody] CategoryUpdateDTO dto)
        {
            var category = await _categoryService.UpdateCategoryAsync(dto);

            if (category == null)
                return NotFound("Category not found");

            return Ok("Category updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            var category = await _categoryService.DeleteCategoryAsync(id);

            if (category == null)
                return NotFound("Category not found");

            return Ok($"Category deleted {category}");
        }
    }
}
