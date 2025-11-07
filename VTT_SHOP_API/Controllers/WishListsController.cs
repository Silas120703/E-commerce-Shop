using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.Services;
using VTT_SHOP_SHARED.DTOs;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListsController : ApiControllerBase
    {
        private readonly WishListService _wishList;

        public WishListsController(WishListService wishList)
        {
            _wishList = wishList;
        }
        [Authorize]
        [HttpGet("get-wish-list")]
        public async Task<IActionResult> GetWishList()
        {
            var UserId = CurrentUserId;
            var result = await _wishList.GetWishListItemAsync(UserId);
            return HandleResult(result);
        }

        [Authorize]
        [HttpPost("add-wish-list-item")]
        public async Task<IActionResult> AddWishListItem( WishListItemDTO itemDTO)
        {
            var UserId = CurrentUserId;
            var result = await _wishList.AddWishListItemAsync(UserId, itemDTO);
            return HandleResult(result);
        }

        [Authorize]
        [HttpDelete("delete-wish-list-item")]
        public async Task<IActionResult> DeleteWishListItem(WishListItemDTO itemDTO)
        {
            var UserId = CurrentUserId;
            var result = await _wishList.RemoveWishListItemAsync(UserId, itemDTO);
            return HandleResult(result);
        }
    }
}
