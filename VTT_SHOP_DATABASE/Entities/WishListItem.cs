using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class WishListItem : EntityBase
    {
        public long WishListId { get; set; }
        public long ProductId { get; set; }
        public WishList? WishList { get; set; }
        public Product? Product { get; set; }
    }
}
