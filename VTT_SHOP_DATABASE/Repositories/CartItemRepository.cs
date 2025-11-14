using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class CartItemRepository : RepositoryBase<CartItem>
    {
        public CartItemRepository(VTTShopDBContext context) : base(context)
        {
        }

        public virtual async Task<List<CartItem>> GetCartItemsAndProductByCartIdAsync(long cartId)
        {
            return await base.GetAll()
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cartId).ToListAsync();
        }

        public virtual async Task<bool> CartItemExistsAsync(long cartId, long producId)
        {
            return await base.GetAll().AnyAsync(c => c.CartId == cartId && c.ProductId == producId);
        }

        public virtual void DeleteCartItemRange( List<CartItem> cartItems)
        {
            base.DeleteRange(cartItems);
        }

        public virtual async Task AddCartItemRange(List<CartItem> cartItems)
        {
            await base.AddRangeAsync(cartItems);
        }

        public virtual async Task<List<CartItem>> GetCartItemDTOsByCartIdAsync(long cartId)
        {
            return await base.GetAll()
                .Include(c => c.Product)
                .Include(c => c.Product!.ProductPictures)
                .Where(c => c.CartId==cartId)
                .ToListAsync();
        }

        public virtual void DeleteCartItem(CartItem cartItem)
        {
            base.Delete(cartItem);
        }

        public virtual void AddCartItem(CartItem cartItem)
        {
            base.Add(cartItem);
        }

        public virtual CartItem UpdateCartItem(CartItem cartItem)
        {
            cartItem.UpdateAt = DateTime.UtcNow;
            return base.Update(cartItem);
        }

        public virtual async Task<CartItem?> GetCartItemByCartIdAndProductIdAsync(long cartId, long productId)
        {
            return await base.GetAll()
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
        public virtual async Task<CartItem?> GetCartItemWithProductByIdAsync(long cartItemId)
        {
            return await base.GetAll()
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }

    }
}
