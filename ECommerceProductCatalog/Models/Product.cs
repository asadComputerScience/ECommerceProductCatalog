﻿namespace ECommerceProductCatalog.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}
