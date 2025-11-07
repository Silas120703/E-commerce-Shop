using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class WishListRepository : RepositoryBase<WishList>
    {
        public WishListRepository(VTTShopDBContext context) : base(context)
        {

        }


        public async Task<WishList?> CreateWishList(WishList wishList)
        {
            return await base.AddAsync(wishList);
        }
        public async Task<bool> WishListExistsAsync(long userId)
        {
            return await base.GetAll().AnyAsync(wl => wl.UserId == userId);
        }

        public async Task<WishList?> GetByUserIdAsync(long userId)
        {
            return await base.GetAll().FirstOrDefaultAsync(wl => wl.UserId == userId);
        }


    }
}
