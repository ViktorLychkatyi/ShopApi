using ShopApplication.DTOs.Category;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Services;

public class CategoryService(ICategoryRepository _repository) : ICategoryService
{
    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        return await _repository.AddCategoryAsync(new Category()
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Url = dto.Url,
            ParentId = dto.ParentId == 0 ? null : dto.ParentId
        });
    }

    public async Task<ICollection<CategoryReadDTO>> GetAllCategoriesAsync()
    {
        var categories = await _repository.GetCategoriesAsync();
        var result = new List<CategoryReadDTO>();

        foreach (var category in categories)
        {
            result.Add(new CategoryReadDTO()
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                ParentId = category.ParentId
            });
        }

        return result;
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        var category = await _repository.GetCategoryAsync(id);

        if (category == null)
            return null;

        return new CategoryReadDTO()
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Url = category.Url = string.IsNullOrEmpty(category.Url) ? "" : category.Url,
            ParentId = category.ParentId
        };
    }

    public async Task<bool> UpdateCategoryAsync(CategoryUpdateDTO dto)
    {
        var category = await _repository.GetCategoryAsync(dto.Id);

        if (category == null)
            return false;

        category.Name = dto.Name;
        category.Slug = dto.Slug;
        category.ParentId = dto.ParentId = dto.ParentId == 0 ? null : dto.ParentId; 
        return await _repository.EditCategoryAsync(category);
    }

    public async Task<int?> DeleteCategoryAsync(int id)
    {
        return await _repository.RemoveCategoryAsync(id);
    }
}
