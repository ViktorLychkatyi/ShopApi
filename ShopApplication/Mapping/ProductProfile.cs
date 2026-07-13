using AutoMapper;
using ShopApplication.DTOs.Product;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<ProductCreateDTO, Product>()
                .ForMember(
                    dest => dest.Images,
                    opt => opt.MapFrom(src =>
                        src.ImageUrls.Select((url, index) => new ProductImage
                        {
                            Url = url,
                            IsPrimary = index == 0
                        }).ToList())
                )
                .ForMember(
                    dest => dest.CategoryId,
                    opt => opt.MapFrom(src =>
                        src.CategoryId == 0 ? null : src.CategoryId)
                );

            CreateMap<Product, ProductReadDTO>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name)
                )
                .ForMember(
                    dest => dest.ImageUrls,
                    opt => opt.MapFrom(src =>
                        src.Images.Select(img => img.Url).ToList()
                    )
                );

            CreateMap<ProductUpdateDTO, Product>();
        }
    }
}
