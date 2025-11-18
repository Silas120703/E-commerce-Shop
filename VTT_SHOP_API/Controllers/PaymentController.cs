using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.Services;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ApiControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("vnpay_return")]
        public IActionResult VnPayReturn()
        {
            var query = HttpContext.Request.Query;

            string redirectUrl = _paymentService.ProcessVnPayReturn(query);

            return Redirect(redirectUrl);
        }

        [HttpGet("vnpay_ipn")]
        public async Task<IActionResult> VnPayIpn()
        {
            var query = HttpContext.Request.Query;

            var ipnResponse = await _paymentService.ProcessVnPayIpnAsync(query);

            return Ok(ipnResponse);
        }
    }
}