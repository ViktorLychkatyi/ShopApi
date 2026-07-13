using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApplication.DTOs.Category
{
    public class CategoryReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? ParentId { get; set; } = null;
        public ICollection<int>? Products { get; set; }
    }
}
