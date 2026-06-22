using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Services
{
    public class CategoryService : ICategoryService
    {
        private List<Category> _categories = new();

        public CategoryService()
        {
            _categories.Add(new Category()
            {
                Title = "Phone",
            });
            _categories.Add(new Category()
            {
                Title = "Phone",
            });
        }

        public void AddCategory(Category category)
        {
            _categories.Add(category);
        }

        public List<Category> GetAllCategoires()
        {
            return _categories;
        }
    }
}
