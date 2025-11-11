using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class CouponUsage : EntityBase
    {
        public long CouponId { get; set; }
        public long UserId { get; set; }
        public long OrderId { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual User User { get; set; }
        public virtual Order Order { get; set; }
    }
}
