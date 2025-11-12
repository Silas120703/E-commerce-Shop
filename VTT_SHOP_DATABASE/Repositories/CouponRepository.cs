using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class CouponRepository : RepositoryBase<Coupon>
    {
        public CouponRepository(VTTShopDBContext context) : base(context)
        {
        }

        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            return await base.GetAll()
                .FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
        }

        public async Task<Coupon> CreateCoupon(Coupon coupon)
        {
            return await base.AddAsync(coupon);
        }

        public Coupon UpdateCoupon (Coupon coupon)
        {
            return  base.Update(coupon);   
        }

        public async Task<bool> IsEligibleAsync(long couponId, decimal orderAmount)
        {
            var coupon = await GetByIdAsync(couponId);
            if (coupon == null || !coupon.IsActive)
            {
                return false;
            }
            var currentDate = DateTime.UtcNow;
            if (currentDate < coupon.StartDate || currentDate > coupon.EndDate)
            {
                return false;
            }
            if (orderAmount < coupon.MinOrderValue)
            {
                return false;
            }
            if (coupon.UsageLimit > 0 && coupon.UsedCount >= coupon.UsageLimit)
            {
                return false;
            }
            return true;
        }

        public void IncrementUsageCount(Coupon coupon)
        {
            coupon.UsedCount += 1;
        }
    }
}
