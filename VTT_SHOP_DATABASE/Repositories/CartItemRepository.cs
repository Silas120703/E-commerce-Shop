using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class CartItemRepository : RepositoryBase<CartItem>
    {
        public CartItemRepository(VTTShopDBContext context) : base(context)
        {
        }

        public async Task<List<CartItem>> GetCartItemsAndProductByCartIdAsync(long cartId)
        {
            return await base.GetAll()
                .Include(ci => ci.Product)
                .Where(ci => ci.CartId == cartId).ToListAsync();
        }

        public async Task<bool> CartItemExistsAsync(long cartId, long producId)
        {
            return await base.GetAll().AnyAsync(c => c.CartId == cartId && c.ProductId == producId);
        }

        public void DeleteCartItemRange( List<CartItem> cartItems)
        {
            base.DeleteRange(cartItems);
        }

        public async Task AddCartItemRange(List<CartItem> cartItems)
        {
            await base.AddRangeAsync(cartItems);
        }

        public async Task<List<CartItemDTO>> GetCartItemDTOsByCartIdAsync(long cartId)
        {
            return await base.GetAll()
                .Where(ci => ci.CartId == cartId)
                .Select(ci => new CartItemDTO
                {
                    CartId = ci.CartId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    ProductName = ci.Product.Name,
                    ProductPrice = ci.Product.Price,
                    ProductPicture = ci.Product.ProductPictures
                        .Where(pp => pp.IsMain)
                        .Select(pp => pp.PictureUrl)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            base.Delete(cartItem);
        }

        public void AddCartItem(CartItem cartItem)
        {
            base.Add(cartItem);
        }

        public CartItem UpdateCartItem(CartItem cartItem)
        {
            cartItem.UpdateAt = DateTime.UtcNow;
            return base.Update(cartItem);
        }

        public async Task<CartItem?> GetCartItemByCartIdAndProductIdAsync(long cartId, long productId)
        {
            return await base.GetAll()
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
        }
        public async Task<CartItem?> GetCartItemWithProductByIdAsync(long cartItemId)
        {
            return await base.GetAll()
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId);
        }

    }
}
