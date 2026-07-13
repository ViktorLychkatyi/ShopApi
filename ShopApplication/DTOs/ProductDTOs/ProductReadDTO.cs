using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.DTOs.Product
{
    public class ProductReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQty { get; set; } = 0;
        public List<string> ImageUrls { get; set; } = new();
        public string CategoryName { get; set; } = string.Empty;
    }
}
