using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class OrderItemRepository : RepositoryBase<OrderItem>
    {
        private readonly VTTShopDBContext _context;

        public OrderItemRepository(VTTShopDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await base.GetAll()
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId).ToListAsync();
        }

        public async Task AddOderItemAsync(List<OrderItem> orderItems)
        {
            await base.AddRangeAsync(orderItems);
        }

        
    }
}
