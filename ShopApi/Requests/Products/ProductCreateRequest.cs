using ShopApplication.DTOs.Product;

namespace ShopApi.Requests.Products
{
    public class ProductCreateRequest : ProductCreateDTO
    {
        public List<IFormFile?> Images { get; set; }
    }
}
