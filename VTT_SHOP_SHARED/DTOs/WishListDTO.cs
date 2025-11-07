namespace VTT_SHOP_SHARED.DTOs
{
    public class WishListDTO
    {
        public long WishListId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductPicture { get; set; }
    }

    public class WishListItemDTO
    {
        public long ProductId { get; set; }
    }

    
}