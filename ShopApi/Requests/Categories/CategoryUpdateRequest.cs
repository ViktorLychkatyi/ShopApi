using ShopApplication.DTOs.CategoryDTOs;

namespace ShopApi.Requests.Categories
{
    public class CategoryUpdateRequest : CategoryUpdateDTO
    {
        public IFormFile? Image { get; set; }
    }
}
