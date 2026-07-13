using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ShopApplication.DTOs.Product
{
    public class ProductCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQty { get; set; } = 0;
        public List<string> ImageUrls { get; set; } = new();
        public int? CategoryId { get; set; }
    }
}
