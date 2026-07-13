using ShopApplication.DTOs.Product;

namespace ShopApi.Requests.Products
{
    public class ProductUpdateRequest : ProductUpdateDTO
    {
        public List<IFormFile> Images { get; set; } = new();
    }
}
