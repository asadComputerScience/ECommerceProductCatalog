namespace ECommerceProductCatalog.DTOs
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string SKU { get; set; }
        public string CategoryName { get; set; }
        public string FirstImagePath { get; set; }
    }



    public class ProductFilterDto
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }

        public int PageNo { get; set; } = 1;     // Dynamic page number
        public int PageSize { get; set; } = 10;  // Dynamic page size
    }


}
