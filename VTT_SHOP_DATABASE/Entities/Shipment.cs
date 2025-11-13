using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class Shipment : EntityBase
    {
        public long OrderId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; } // "GHN", "ViettelPost", ...
        public string Status { get; set; } // "PendingPreparation", "Shipped", "Delivered"
        public DateTime? ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public virtual Order Order { get; set; }
    }
}
