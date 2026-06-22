using ShopDomain.Models;

namespace ShopApp.Interfaces
{
    public interface ICategoryService
    {
        List<Category> GetAllCategoires();
        void AddCategory(Category category);
    }
}
