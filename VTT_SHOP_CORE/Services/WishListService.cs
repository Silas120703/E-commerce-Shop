using AutoMapper;
using FluentResults;
using Org.BouncyCastle.Tsp;
using VTT_SHOP_CORE.Errors;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_CORE.Services
{
    public class WishListService : ServiceBase<WishList>
    {
        private readonly WishListRepository _wishList;
        private readonly WishListItemRepository _wishListItem;
        private readonly UserRepository _user;
        private readonly ProductRepository _product;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public WishListService(WishListRepository wishList, WishListItemRepository wishListItem, UserRepository user, ProductRepository product, IMapper mapper, IUnitOfWork unitOfWork) : base(wishList)
        {
            _wishList = wishList;
            _wishListItem = wishListItem;
            _user = user;
            _product = product;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List< WishListDTO>>> GetWishListItemAsync(long userId)
        {
            var wistList = await _wishList.GetByUserIdAsync(userId);
            if (wistList == null)
            {
                return Result.Fail(new NotFoundError($"Wishlist not found with userId {userId}"));
            }
            var wishListItems = await _wishListItem.GetItemsByWishListIdAsync(wistList.Id);
            return   Result.Ok(_mapper.Map<List<WishListDTO>>(wishListItems));
        }

        public async Task<Result<WishListDTO>> AddWishListItemAsync(long userId, WishListItemDTO itemDTO)
        {
            if (await _user.GetByIdAsync(userId) == null)
            {
                return Result.Fail(new NotFoundError("User not found"));
            }
            var product = await _product.GetByIdAsync(itemDTO.ProductId);
            if (product == null)
            {
                return Result.Fail(new NotFoundError("Product not found"));
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var wishList = await _wishList.GetByUserIdAsync(userId);
                if (wishList == null)
                {
                    wishList = new WishList { UserId = userId };
                    wishList = await _wishList.AddAsync(wishList);
                    await _unitOfWork.SaveChangesAsync();
                }
                var wishListItem = new WishListItem
                {
                    WishListId = wishList.Id,
                    ProductId = itemDTO.ProductId
                };
                if (await _wishListItem.IsProductInWishListAsync(wishList.Id, itemDTO.ProductId))
                {
                    return Result.Ok(_mapper.Map<WishListDTO>(wishListItem));
                }
                var newWishListItem = await _wishListItem.AddWishListItemAsync(wishListItem);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return Result.Ok(_mapper.Map<WishListDTO>(newWishListItem))
                             .WithSuccess("Create successful wish list item");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail("An error occurred while adding the wish list item.: " + ex.Message);

            }
        }

        public async Task<Result> RemoveWishListItemAsync(long userId, WishListItemDTO itemDTO)
        {
            if (await _user.GetByIdAsync(userId) == null)
            {
                return Result.Fail(new NotFoundError("User not found"));
            }
            var wishList = await _wishList.GetByUserIdAsync(userId);
            if (wishList == null)
            {
                return Result.Fail(new NotFoundError("WishList not found"));
            }
            try
            {
                var wishListItems = await _wishListItem.GetWishListItemByWishListIdAndProductId(wishList.Id,itemDTO.ProductId);
                if (wishListItems == null)
                {
                    return Result.Fail(new NotFoundError("No items in the wish list"));
                }
                _wishListItem.RemoreWishListItem(wishListItems);
                await _unitOfWork.SaveChangesAsync();
                return Result.Ok().WithSuccess("Remove successful wish list item");

            }
            catch (Exception ex)
            {
                return Result.Fail("An error occurred while removing the wish list item.: " + ex.Message);
            }
        }
    }
}