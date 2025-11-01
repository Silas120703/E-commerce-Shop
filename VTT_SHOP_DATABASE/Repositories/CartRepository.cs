using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class CartRepository : RepositoryBase<Cart>
    {
        public CartRepository(VTTShopDBContext context) : base(context)
        {
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await base.GetAll().FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<Cart?> GetCartWithItemsByUserIdAsync(int userId)
        {
            return await base.GetAll()
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
        public async Task<bool> CartExistsAsync(int userId)
        {
            return await base.GetAll().AnyAsync(c => c.UserId == userId);
        }

        public async Task<Cart?> CreateCartAsync(Cart cart)
        {
            await base.AddAsync(cart);
            return cart;
        }



    }
}
