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
                Id = 1,
                Title = "Phone 1",
                Price = 1000
            });
            _products.Add(new Product()
            {
                Id = 2,
                Title = "Phone 2",
                Price = 2000
            });
            _products.Add(new Product()
            {
                Id = 3,
                Title = "Phone 3",  
                Price = 3000
            });
            _products.Add(new Product()
            {
                Id = 4,
                Title = "Phone 4",
                Price = 4000
            });
        }

        public Product? GetProduct(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        //public void MarkAsViewed(int id)
        //{
        //    var product = _products.FirstOrDefault(p => p.Id == id);
        //    if (product != null)
        //        product.IsViewed = true;
        //}

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Product? UpdateProduct(int id, Product newProduct)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            _products.Remove(product);
        }

        public Product? SearchProduct(string? title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return null;

            return _products.FirstOrDefault(p => p.Title != null && p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }
    }
}
