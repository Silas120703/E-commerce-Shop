using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class PaymentRepository : RepositoryBase<Payment>
    {
        public PaymentRepository(VTTShopDBContext context) : base(context)
        {
        }

        public virtual async Task<Payment?> GetByOrderIdAsync(long orderId)
        {
            return await base.GetAll().FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
}