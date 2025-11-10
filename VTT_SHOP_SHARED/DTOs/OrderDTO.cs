namespace VTT_SHOP_SHARED.DTOs
{
    public class OrderItemCreateDTO
    {
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderItemDTO
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal PriceTotal => Price * Quantity;
    }

    public class OrderDetailDTO
    {
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }
}
