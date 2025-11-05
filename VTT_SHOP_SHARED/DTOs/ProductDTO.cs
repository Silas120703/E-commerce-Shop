namespace VTT_SHOP_SHARED.DTOs
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SlugName { get; set; } = string.Empty ;
        public string Description { get; set; } = string.Empty ;
        public int Quantity { get; set; } 
        public decimal Price { get; set; }
        public string ProductPicture { get; set; }
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

    public class UpdateProductPictureDTO
    {
        public long ProductId { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
    }
}
