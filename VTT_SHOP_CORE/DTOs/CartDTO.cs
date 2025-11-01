namespace VTT_SHOP_CORE.DTOs
{
    public class CartDTO
    {
        public long CartId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductPicture { get; set; }
    }

    public class CartCreateDTO
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
