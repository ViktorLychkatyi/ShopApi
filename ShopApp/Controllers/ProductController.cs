using Microsoft.AspNetCore.Mvc;
using Shop.App.Filters;
using ShopApp.Interfaces;
using ShopDomain.Models;
//using ShopApp.Services;

namespace ShopApp.Controllers
{
    // http://localhost:port/api/product
    // http://localhost:port/product


    [ApiController]
    [Route("api/[controller]")]
    [LogActionFilter]
    public class ProductController(IProductService _productService) : ControllerBase
    {
        //private readonly IProductService _productService;

        //public ProductController(IProductService productService)
        //{
        //    _productService = productService;
        //}

        [HttpGet]
        public List<Product> GetProducts()
        {
            return _productService.GetAllProducts();
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var product = _productService.GetProduct(id);

            if (product == null)
                return NotFound("Product not found");

            if (string.IsNullOrWhiteSpace(product.Title))
                return BadRequest("Product null");

            _productService.MarkAsViewed(id);

            return Ok(product);
        }

        [HttpPost]
        public IActionResult AddNewProduct([FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product?.Title))
                return BadRequest("Product null");

            var products = _productService.GetAllProducts();
            int nextId = products.Max(p => p.Id) + 1;
            product.Id = nextId;

            _productService.AddProduct(product);

            return Created($"/api/products/{product.Id}", product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProductById(int id, [FromBody] Product newProduct) 
        {
            var products = _productService.GetAllProducts();
            var findProduct = products.FirstOrDefault(p => p.Id == id);

            if (findProduct == null)
                return NotFound("Product not found");

            if (string.IsNullOrWhiteSpace(findProduct.Title))
                return BadRequest("Product null");

            findProduct.Title = newProduct.Title;
            findProduct.Price = newProduct.Price;

            _productService.UpdateProduct(id, newProduct);
            return Ok(findProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProductById([FromRoute] int id)
        {
            var products = _productService.GetAllProducts();
            var findProduct = products.FirstOrDefault(p => p.Id == id);

            if (findProduct == null)
                return NotFound("Product not found");

            if (string.IsNullOrWhiteSpace(findProduct.Title))
                return BadRequest("Product null");

            _productService.DeleteProduct(id);
            return NoContent();
        }

        [HttpGet("search")]
        public IActionResult SearchItem([FromQuery] string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Title is required");

            var products = _productService.GetAllProducts();

            var product = products.FirstOrDefault(p => p.Title != null && p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

            if (product == null)
                return NotFound("Product not found");

            return Ok(product);
        }
    }
}
