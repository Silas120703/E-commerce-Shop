using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class CouponUsageRepository : RepositoryBase<CouponUsage>
    {
        public CouponUsageRepository(VTTShopDBContext context) : base(context)
        {
        }

        public virtual async Task<int> CountByUserIdAndCouponIdAsync(long userId, long couponId)
        {
            return await base.GetAll()
                .CountAsync(cu => cu.UserId == userId && cu.CouponId == couponId);
        }
    }
}
