namespace ShopDomain.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public float Price { get; set; }
        public bool IsViewed { get; set; }
    }
}
