using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class Order : EntityBase
    {
        public long UserID { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public long ShippingAddressId { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual User User { get; set; }
    }
}
