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
        public long Id { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; } 
        public decimal FinalAmount { get; set; } 
        public DateTime CreateAt { get; set; }
        public List<OrderItemDTO> Items { get; set; }
    }

    public class CreateOrderDTO
    {
        public long ShippingAddressId { get; set; }
        public string? CouponCode { get; set; } 
    }

    public class CreateOrderWithItemsDTO
    {
        public CreateOrderDTO createOrderDTO { get; set; }
        public OrderItemCreateDTO OrderItemCreateDTO { get; set; }
    }
}
