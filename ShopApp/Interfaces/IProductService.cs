using ShopDomain.Models;

namespace ShopApp.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product? GetProduct(int id);
        void AddProduct(Product product);
        Product? UpdateProduct(int id, Product newProduct);
        void DeleteProduct(int id);
        Product? SearchProduct(string? title);
        void MarkAsViewed(int id);
    }
}
