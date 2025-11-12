namespace VTT_SHOP_SHARED.DTOs
{
    public class CouponDTO
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; } // "Percentage" or "FixedAmount"
        public decimal DiscountValue { get; set; }
        public decimal MaxDiscountValue { get; set; }
        public decimal MinOrderValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsageLimitPerUser { get; set; }
    }

    public class CouponDetailDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string DiscountType { get; set; } // "Percentage" or "FixedAmount"
        public decimal DiscountValue { get; set; }
        public decimal MaxDiscountValue { get; set; }
        public decimal MinOrderValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsageLimitPerUser { get; set; }
        public int UsedCount { get; set; }
    }
}
