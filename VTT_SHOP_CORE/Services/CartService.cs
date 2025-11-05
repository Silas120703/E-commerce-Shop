using AutoMapper;
using FluentResults;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_CORE.Services
{
    public class CartService : ServiceBase<Cart>
    {
        private readonly CartRepository _cart;
        private readonly CartItemRepository _cartItem;
        private readonly UserRepository _user;
        private readonly ProductRepository _product;
        private readonly ProductPictureRepository _productPiture;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(CartRepository cart, CartItemRepository cartItem, UserRepository user, ProductRepository product, ProductPictureRepository productPicture, IMapper mapper, IUnitOfWork unitOfWork) : base(cart)
        {
            _cart = cart;
            _cartItem = cartItem;
            _user = user;
            _product = product;
            _productPiture = productPicture;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CartItemDTO>>> GetCartItemsByUserIdAsync(long userId)
        {
            if (await _user.GetByIdAsync(userId) == null)
            {
                return Result.Fail<List<CartItemDTO>>("User not found");
            }

            var cart = await _cart.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return Result.Ok(new List<CartItemDTO>());
            }
            var cartItems = await _cartItem.GetCartItemDTOsByCartIdAsync(cart.Id);
            var cartItemDTOs = _mapper.Map<List<CartItemDTO>>(cartItems);
            return Result.Ok(cartItemDTOs);
        }

        public async Task<Result<CartItemDTO>> AddCartItem(long userId, CartItemCreateDTO cartItemDto)
        {

            if (await _user.GetByIdAsync(userId) == null)
            {
                return Result.Fail("User not found");
            }
            var product = await _product.GetByIdAsync(cartItemDto.ProductId);
            if (product==null)
            {
                return Result.Fail("Product not found");
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var cart = await _cart.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId
                    };
                    await _cart.AddAsync(cart);
                    await _unitOfWork.SaveChangesAsync();
                }
                var existingCartItem = await _cartItem.GetCartItemByCartIdAndProductIdAsync(cart.Id,  cartItemDto.ProductId);
                CartItem itemToReturn;
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cartItemDto.Quantity;
                    _cartItem.UpdateCartItem(existingCartItem);
                    itemToReturn = existingCartItem;
                }
                else
                {
                    var newCartItem = _mapper.Map<CartItem>(cartItemDto);
                    newCartItem.CartId = cart.Id;
                    itemToReturn = await _cartItem.AddAsync(newCartItem);
                }
                await _unitOfWork.SaveChangesAsync();
                itemToReturn.Product = product;
                var cartItemResult = _mapper.Map<CartItemDTO>(itemToReturn);
                await _unitOfWork.CommitAsync();
                return Result.Ok(cartItemResult);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail($"An error occurred while adding the cart item: {ex.Message}");
            }
        }

        public async Task<Result> DeleteCartItem(long userId, CartItemDeleteDTO cartItemDelete)
        {
            if (await _user.GetByIdAsync(userId) == null)
            {
                return Result.Fail("User not found");
            }


            var cart = await _cart.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return Result.Fail("Cart not found");
            }

            var existingCartItem = await _cartItem.GetCartItemByCartIdAndProductIdAsync(cart.Id, cartItemDelete.ProductId);

            if (existingCartItem == null)
            {
                return Result.Fail("Cart item not found"); 
            }

            try
            {
                _cartItem.Delete(existingCartItem);
                await _unitOfWork.SaveChangesAsync();

                return Result.Ok().WithSuccess("Delete cart item successfully");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred during deletion: {ex.Message}");
            }
        }
    }
}
