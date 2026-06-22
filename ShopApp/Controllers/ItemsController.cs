using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private List<Product> _products = new();

        [HttpGet]
        public List<Product> GetProducts()
        {
            _products.Add(new Product()
            {
                Title = "Phone",
                Price = 1000
            });
            _products.Add(new Product()
            {
                Title = "Phone",
                Price = 2000
            });

            return _products;
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var product = new Product()
            {
                Title = $"Test Product {id}",
                Price = 100
            };
            return Ok(product);
        }
    }
}
