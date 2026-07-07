using ShopApplication.DTOs.Category;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.DTOs.Product;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Services
{
    public class ProductService(IProductRepository _repository) : IProductService
    {
        public async Task<int?> CreateProductAsync(ProductCreateDTO dto)
        {
            var product = new Product()
            {
                Name = dto.Name,
                Price = dto.Price,
                StockQty = dto.StockQty,
                CategoryId = dto.CategoryId,
                Images = dto.ImageUrls.Select(url => new ProductImage
                {
                    Url = url
                }).ToList()
            };

            return await _repository.AddProductAsync(product);
        }

        public async Task<ICollection<ProductReadDTO>> GetAllProductsAsync()
        {
            var products = await _repository.GetProductsAsync();
            var result = new List<ProductReadDTO>();

            foreach (var product in products)
            {
                result.Add(new ProductReadDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    StockQty = product.StockQty,
                    CategoryName = product.Category?.Name,
                    ImageUrls = product.Images
                        .Select(img => img.Url)
                        .ToList()
                });
            }

            return result;
        }

        public async Task<ProductReadDTO?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetProductAsync(id);

            if (product == null)
                return null;

            return new ProductReadDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQty = product.StockQty,
                CategoryName = product.Category?.Name,
                ImageUrls = product.Images
                    .Select(img => img.Url)
                    .ToList()
            };
        }

        public async Task<bool> UpdateProductAsync(ProductUpdateDTO dto)
        {
            var product = await _repository.GetProductAsync(dto.Id);

            if (product == null)
                return false;

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.StockQty = dto.StockQty;
            _ = product.CategoryId == 0 ? null : dto.CategoryId;

            if (dto.ImageUrls.Any())
            {
                if (dto.ImageUrls == null)
                {
                    //Console.WriteLine("-");
                }    
                else
                {
                    _repository.RemoveImages(product.Images);
                    product.Images.Clear();

                    foreach (var url in dto.ImageUrls)
                    {
                        product.Images.Add(new ProductImage
                        {
                            Url = url,
                            ProductId = product.Id
                        });
                    }
                }
            }


            return await _repository.EditProductAsync(product);
        }

        public async Task<int?> DeleteProductAsync(int id)
        {
            return await _repository.RemoveProductAsync(id);
        }
    }
}
