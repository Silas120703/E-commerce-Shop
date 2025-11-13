using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class Payment : EntityBase
    {
        public long OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // "VNPay", "COD", ...
        public string Status { get; set; } // "pending", "success", "failed"
        public DateTime? TransactionDate { get; set; }

        public virtual Order Order { get; set; }
    }
}
