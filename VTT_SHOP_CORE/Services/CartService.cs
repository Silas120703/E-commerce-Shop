using AutoMapper;
using FluentResults;
using VTT_SHOP_CORE.DTOs;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartService(CartRepository cart, CartItemRepository cartItem, UserRepository user, IMapper mapper, IUnitOfWork unitOfWork) : base(cart)
        {
            _cart = cart;
            _cartItem = cartItem;
            _user = user;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _cart.GetCartWithItemsByUserIdAsync(userId);
        }

        //public async Task<Result<CartDTO>> AddCartItem(int userId, CartCreateDTO cartItemDto)
        //{
            
        //}
    }
}
