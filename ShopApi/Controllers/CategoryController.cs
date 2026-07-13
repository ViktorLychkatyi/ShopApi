using Microsoft.AspNetCore.Http.HttpResults;
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
using static System.Net.Mime.MediaTypeNames;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoryController(ICategoryService _categoryService, IImageService _imageService, IConfiguration _configuration, IConfiguration _mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest dto)
        {
            var maxImages = _configuration.GetValue<int>("FileSettings:MaxProductImages");
            var maxSizeMb = _configuration.GetValue<int>("FileSettings:MaxFileSizeMb");
            var maxSizeBytes = maxSizeMb * 1024 * 1024;

            var allowedExtensions = _configuration
                .GetSection("FileSettings:AllowedExtensions")
                .Get<string[]>();

            dto.Url = await _imageService.SaveFileAsync(dto.Image, _configuration["DirnameForFiles:Categories"]);

            if (dto.Image.Length > maxSizeBytes)
                return BadRequest($"Maximum file size is {maxSizeMb} MB.");

            var createDto = new CategoryCreateDTO
            {
                Name = dto.Name,
                Url = dto.Url,
                Slug = dto.Slug,
                ParentId = dto.ParentId == 0 ? null : dto.ParentId,
            };

            var id = await _categoryService.CreateCategoryAsync(createDto);

            return Ok($"Category created {id}");
        }

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

        [HttpGet("parent")]
        public async Task<IActionResult> GetParentCategories()
        {
            var categories = await _categoryService.GetCategoriesByParentAsync();
            return Ok(categories);
        }

        [HttpGet("child")]
        public async Task<IActionResult> GetChildCategories()
        {
            var categories = await _categoryService.GetCategoriesByChildAsync();
            return Ok(categories);
        }

        [HttpGet("tree")]
        public async Task<IActionResult> GetTreeCategories()
        {
            var categories = await _categoryService.GetCategoriesByTreeAsync();
            return Ok(categories);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryById(int id, [FromForm] CategoryUpdateRequest dto)
        {
            var maxImages = _configuration.GetValue<int>("FileSettings:MaxProductImages");
            var maxSizeMb = _configuration.GetValue<int>("FileSettings:MaxFileSizeMb");
            var maxSizeBytes = maxSizeMb * 1024 * 1024;

            var allowedExtensions = _configuration
                .GetSection("FileSettings:AllowedExtensions")
                .Get<string[]>();

            var extension = Path.GetExtension(dto.Image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type.");

            if (dto.Image.Length > maxSizeBytes)
                return BadRequest($"Maximum file size is {maxSizeMb} MB.");

            dto.Url = await _imageService.SaveFileAsync(dto.Image, _configuration["DirnameForFiles:Categories"]);

            var updateDto = new CategoryUpdateDTO
            {
                Id = id,
                Name = dto.Name,
                Slug = dto.Slug,
                Url = dto.Url,
                ParentId = dto.ParentId == 0 ? null : dto.ParentId
            };

            var category = await _categoryService.UpdateCategoryAsync(updateDto);

            if (!category)
                return NotFound("Category not found.");

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
