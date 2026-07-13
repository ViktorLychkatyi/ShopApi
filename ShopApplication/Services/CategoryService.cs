using AutoMapper;
using ShopApplication.DTOs.Category;
using ShopApplication.DTOs.CategoryDTOs;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Services;

public class CategoryService(ICategoryRepository _repository, IMapper _mapper) : ICategoryService
{
    public async Task<int?> CreateCategoryAsync(CategoryCreateDTO dto)
    {
        var category = _mapper.Map<Category>(dto);
        return await _repository.AddCategoryAsync(category);
    }

    public async Task<ICollection<CategoryReadDTO>> GetAllCategoriesAsync()
    {
        var categories = await _repository.GetCategoriesAsync();
        List<CategoryReadDTO> dtos = null;
        if (categories != null && categories.Count > 0)
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);
        return dtos;
    }

    public async Task<CategoryReadDTO?> GetCategoryByIdAsync(int id)
    {
        CategoryReadDTO? dto = null;
        var category = await _repository.GetCategoryAsync(id);
        if (category != null)
            dto = _mapper.Map<CategoryReadDTO>(category);
        return dto;
    }

    public async Task<ICollection<CategoryReadDTO>> GetCategoriesByParentAsync()
    {
        var categories = await _repository.GetParentCategoriesAsync();
        List<CategoryReadDTO> dtos = null;
        if (categories != null && categories.Count > 0)
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);
        return dtos;
    }

    public async Task<ICollection<CategoryReadDTO>> GetCategoriesByChildAsync()
    {
        var categories = await _repository.GetChildCategoriesAsync();
        List<CategoryReadDTO> dtos = null;
        if (categories != null && categories.Count > 0)
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);
        return dtos;
    }

    public async Task<ICollection<CategoryReadDTO>> GetCategoriesByTreeAsync()
    {
        var categories = await _repository.GetTreeCategoriesAsync();
        List<CategoryReadDTO> dtos = null;
        if (categories != null && categories.Count > 0)
            dtos = _mapper.Map<List<CategoryReadDTO>>(categories);
        return dtos;
    }

    public async Task<bool> UpdateCategoryAsync(CategoryUpdateDTO dto)
    {
        var category = await _repository.GetCategoryAsync(dto.Id);
        if (category == null)
            return false;
        _mapper.Map(dto, category);
        category.ParentId = dto.ParentId = dto.ParentId == 0 ? null : dto.ParentId;
        return await _repository.EditCategoryAsync(category);
    }

    public async Task<int?> DeleteCategoryAsync(int id)
    {
        return await _repository.RemoveCategoryAsync(id);
    }
}
