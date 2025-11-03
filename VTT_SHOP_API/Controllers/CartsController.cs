using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.Services;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ApiControllerBase
    {
        private readonly CartService _cartService;

        public CartsController(CartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        [HttpGet("cart-items")]
        public async Task<IActionResult> GetCartItems()
        {
            var userId = CurrentUserId;
            var result = await _cartService.GetCartItemsByUserIdAsync(userId);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
            }
        }

        [Authorize]
        [HttpPost("add-cart-item")]
        public async Task<IActionResult> AddCartItem(CartItemCreateDTO cartItemDto)
        {
            var userId = CurrentUserId;
            var result = await _cartService.AddCartItem(userId, cartItemDto);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
            }
        }

        [Authorize]
        [HttpDelete("delete-cart-item")]
        public async Task<IActionResult> DeleteCartItem([FromBody] CartItemDeleteDTO cartItemDeleteDto)
        {
            var userId = CurrentUserId;
            var result = await _cartService.DeleteCartItem(userId, cartItemDeleteDto);
            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
            }
        }

    }
}
