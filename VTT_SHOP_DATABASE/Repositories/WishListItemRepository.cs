using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class WishListItemRepository : RepositoryBase<WishListItem>
    {
        public WishListItemRepository( VTTShopDBContext context) : base(context) 
        {
            
        }

    }
}
