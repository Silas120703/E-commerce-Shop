using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class OrderItem : EntityBase
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
