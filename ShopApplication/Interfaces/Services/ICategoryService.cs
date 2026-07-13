using ShopApplication.DTOs.Category;
using ShopApplication.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<int?> CreateCategoryAsync(CategoryCreateDTO dto);
        Task<ICollection<CategoryReadDTO>> GetAllCategoriesAsync();
        Task<CategoryReadDTO?> GetCategoryByIdAsync(int id);
        Task<bool> UpdateCategoryAsync(CategoryUpdateDTO dto);
        Task<int?> DeleteCategoryAsync(int id);
        Task<ICollection<CategoryReadDTO>> GetCategoriesByParentAsync();
        Task<ICollection<CategoryReadDTO>> GetCategoriesByChildAsync();
        Task<ICollection<CategoryReadDTO>> GetCategoriesByTreeAsync();
    }
}
