using ShopApplication.DTOs.Category;

namespace ShopApi.Requests.Categories
{
    public class CategoryCreateRequest : CategoryCreateDTO
    {
        public IFormFile? Image { get; set; }
    }
}
