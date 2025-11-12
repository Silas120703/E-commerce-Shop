using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class Order : EntityBase
    {
        public long UserId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; } 
        public decimal FinalAmount { get; set; }
        public long ShippingAddressId { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
