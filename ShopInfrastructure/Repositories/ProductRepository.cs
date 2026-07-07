using Microsoft.EntityFrameworkCore;
using ShopApplication.Interfaces.Repository;
using ShopDomain.Models;
using ShopInfrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopInfrastructure.Repositories
{
    public class ProductRepository(ShopDbContext _context) : IProductRepository
    {
        public async Task<int?> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<ICollection<Product>> GetProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();
            return products;
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            return product;
        }

        public async Task<bool> EditProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int?> RemoveProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return null;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return id;
        }

        public void RemoveImages(ICollection<ProductImage> images)
        {
            _context.ProductImages.RemoveRange(images);
        }
    }
}
