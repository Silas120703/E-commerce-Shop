namespace VTT_SHOP_CORE.DTOs
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SlugName { get; set; } = string.Empty ;
        public string Description { get; set; } = string.Empty ;
        public int Quantity { get; set; } 
        public decimal Price { get; set; }
    }

    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class UpdateProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
