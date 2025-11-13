using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.Services;
using VTT_SHOP_SHARED.DTOs;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ApiControllerBase
    {
        private readonly CouponService _couponService;

        public CouponsController(CouponService couponService) 
        {
            _couponService = couponService;
        }

        [HttpGet("get-by-code")]
        public async Task<IActionResult> GetCouponByCode([FromQuery] string code)
        {
            var result = await _couponService.GetCouponByCodeAsync(code);
            return HandleResult(result);
        }

        [HttpPost("create-coupon")]
        public async Task<IActionResult> CreateCoupon(CouponDTO couponDto)
        {
            var result = await _couponService.CreateCouponAsync(couponDto);
            return HandleResult(result);
        }

        [HttpPut("update-coupon/{id}")]
        public async Task<IActionResult> UpdateCoupon(long id, CouponDTO couponDto)
        {
            var result = await _couponService.UpdateCouponAsync(id, couponDto);
            return HandleResult(result);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetCouponById(long id)
        {
            var result = await _couponService.GetCouponByIdAsync(id);
            return HandleResult(result);
        }

        [HttpPatch("deactivate-coupon/{id}")]
        public async Task<IActionResult> DeactivateCoupon(long id)
        {
            var result = await _couponService.DeactiveCouponAsync(id);
            return HandleResult(result);
        }

        [HttpPatch("activate-coupon/{id}")]
        public async Task<IActionResult> ActivateCoupon(long id)
        {
            var result = await _couponService.ActiveCouponAsync(id);
            return HandleResult(result);
        }

    }
}
