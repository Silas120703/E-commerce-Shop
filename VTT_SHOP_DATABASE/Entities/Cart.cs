using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class Cart : EntityBase
    {
        public long UserId { get; set; }
        public User? User { get; set; }
        public virtual ICollection<CartItem>? Items { get; set; }
    }
}