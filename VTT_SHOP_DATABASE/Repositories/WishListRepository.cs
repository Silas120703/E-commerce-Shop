using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class WishListRepository : RepositoryBase<WishList>
    {
        public WishListRepository(VTTShopDBContext context) : base(context) 
        {
            
        }

    }
}
