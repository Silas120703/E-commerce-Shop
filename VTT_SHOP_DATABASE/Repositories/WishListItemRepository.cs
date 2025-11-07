using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class WishListItemRepository : RepositoryBase<WishListItem>
    {
        public WishListItemRepository(VTTShopDBContext context) : base(context)
        {

        }

        public async Task<List<WishListItem>> GetItemsByWishListIdAsync(long wishListId)
        {
            return await base.GetAll()
                .Include(wli => wli.Product)
                .ThenInclude(p => p.ProductPictures)
                .Where(wli => wli.WishListId == wishListId)
                .ToListAsync();
        }

        public async Task<WishListItem?> AddWishListItemAsync(WishListItem wishListItem)
        {
            return await base.AddAsync(wishListItem);
        }

        public void  RemoreWishListItem(WishListItem wishListItem)
        {
             base.Delete(wishListItem);
        }

        public async Task<WishListItem?> GetWishListItemByWishListIdAndProductId(long wishListId, long productId)
        {
            return await  base.GetAll()
                .FirstOrDefaultAsync(wli => wli.WishListId == wishListId && wli.ProductId == productId);
        }

        public async Task<bool> IsProductInWishListAsync(long id, long productId)
        {
            return await base.GetAll()
                .AnyAsync(wli => wli.WishListId == id && wli.ProductId == productId);
        }
    }
}
