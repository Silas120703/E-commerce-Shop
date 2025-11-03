namespace VTT_SHOP_SHARED.DTOs
{
    public class CartItemDTO
    {
        public long CartId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductPicture { get; set; }
    }

    public class CartItemCreateDTO
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemDeleteDTO
    {
        public long ProductId { get; set; }
    }
}
