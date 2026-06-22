using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Services
{
    public class ProductService : IProductService
    {
        private List<Product> _products = new();

        public ProductService()
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
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }
    }
}
