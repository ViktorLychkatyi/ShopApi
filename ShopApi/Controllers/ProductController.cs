using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Filters;
using ShopApi.Requests.Products;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.DTOs.Product;
using ShopApplication.Interfaces;
using ShopApplication.Interfaces.Services;
using ShopApplication.Services;
using ShopDomain.Models;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController(IProductService _productService, IImageService _imageService, IConfiguration _configuration) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest dto)
        {
            var maxImages = _configuration.GetValue<int>("FileSettings:MaxProductImages");
            var maxSizeMb = _configuration.GetValue<int>("FileSettings:MaxFileSizeMb");
            var maxSizeBytes = maxSizeMb * 1024 * 1024;

            var allowedExtensions = _configuration
                .GetSection("FileSettings:AllowedExtensions")
                .Get<string[]>();

            if (dto.Images.Count > maxImages)
                return BadRequest($"You can upload up to {maxImages} images.");

            var createDto = new ProductCreateDTO
            {
                Name = dto.Name,
                Price = dto.Price,
                StockQty = dto.StockQty,
                CategoryId = dto.CategoryId == 0 ? null : dto.CategoryId,
                ImageUrls = new List<string>()
            };

            foreach (var image in dto.Images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("Invalid file type.");

                var imageUrl = await _imageService.SaveFileAsync(image, _configuration["DirnameForFiles:Products"]);

                if (image.Length > maxSizeBytes)
                    return BadRequest($"Maximum file size is {maxSizeMb} MB.");

                if (!string.IsNullOrEmpty(imageUrl))
                    createDto.ImageUrls.Add(imageUrl);
            }

            var id = await _productService.CreateProductAsync(createDto);

            return Ok($"Product created {id}");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductById(int id, [FromForm] ProductUpdateRequest dto)
        {
            var product = await _productService.UpdateProductAsync(dto);

            var maxImages = _configuration.GetValue<int>("FileSettings:MaxProductImages");
            var maxSizeMb = _configuration.GetValue<int>("FileSettings:MaxFileSizeMb");
            var maxSizeBytes = maxSizeMb * 1024 * 1024;

            var allowedExtensions = _configuration
                .GetSection("FileSettings:AllowedExtensions")
                .Get<string[]>();

            if (id != dto.Id)
                return NotFound("Product not found");

            if (dto.Images.Count > maxImages)
                return BadRequest($"You can upload up to {maxImages} images.");

            var updateDto = new ProductUpdateDTO
            {
                Id = id,
                Name = dto.Name,
                Price = dto.Price,
                StockQty = dto.StockQty,
                CategoryId = dto.CategoryId == 0 ? null : dto.CategoryId,
                ImageUrls = new List<string>()
            };

            foreach (var image in dto.Images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return BadRequest("Invalid file type.");

                var imageUrl = await _imageService.SaveFileAsync(image, _configuration["DirnameForFiles:Products"]);

                if (image.Length > maxSizeBytes)
                    return BadRequest($"Maximum file size is {maxSizeMb} MB.");

                if (!string.IsNullOrEmpty(imageUrl))
                    updateDto.ImageUrls.Add(imageUrl);
            }

            var result = await _productService.UpdateProductAsync(updateDto);
            return Ok("Product updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            var product = await _productService.DeleteProductAsync(id);

            if (product == null)
                return NotFound("Product not found");

            return Ok($"Product deleted {product}");
        }
    }
}
