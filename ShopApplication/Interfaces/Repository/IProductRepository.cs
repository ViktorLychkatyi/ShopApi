using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces.Repository
{
    public interface IProductRepository
    {
        Task<int?> AddProductAsync(Product product);
        Task<ICollection<Product>> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<bool> EditProductAsync(Product product);
        Task<int?> RemoveProductAsync(int id);
        void RemoveImages(ICollection<ProductImage> images);
    }
}
