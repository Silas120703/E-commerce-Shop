using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class OrderRepository : RepositoryBase<Order>
    {
        private readonly VTTShopDBContext _context;

        public OrderRepository(VTTShopDBContext context) : base(context)
        { 
            _context = context;
        }

        public virtual async Task<Order> CreateOrderAsync(Order order)
        {
            return await base.AddAsync(order);
        }

        public virtual  Order UpdateOrder(Order order)
        {
            return base.Update(order);
        }

        public virtual async Task<List<Order>> GetOrdersByUserIdAsync(long userId)
        {
            return await base.GetAll()
                .Include(x => x.Items)
                .ThenInclude(p => p.Product)
                .Where(o => o.UserId == userId).ToListAsync();
        }

    }
}
