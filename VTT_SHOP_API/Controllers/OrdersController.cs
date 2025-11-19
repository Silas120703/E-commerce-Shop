using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.Services;
using VTT_SHOP_SHARED.DTOs;
using FluentResults; // <-- Thêm

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ApiControllerBase
    {
        private readonly OrderService _orderService;
        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("my-orders")] 
        public async Task<IActionResult> GetMyOrders([FromQuery] PagingParams pagingParams)
        {
            var userId = CurrentUserId;
            var result = await _orderService.GetUserOrdersPagedAsync(userId, pagingParams);
            return HandleResult(result);
        }

        [HttpPost("create-from-cart")]
        public async Task<IActionResult> CreateOrderFromCart([FromBody] CreateOrderDTO createOrderDto)
        {
            var userId = CurrentUserId;
            var ipAddress = CurrentUserIpAddress; 

            var result = await _orderService.CreateOrderFromCartAsync(userId, createOrderDto, ipAddress);

            return HandleResult(result);
        }

        [HttpPost("create-from-product")]
        public async Task<IActionResult> CreateOrderFromProduct(CreateOrderWithItemsDTO model)
        {
            var userId = CurrentUserId;
            var ipAddress = CurrentUserIpAddress; 

            var result = await _orderService.CreateOrderFromProductAsync(userId, model.OrderItemCreateDTO, model.createOrderDTO, ipAddress);

            return HandleResult(result);
        }

        [HttpGet("get-order-detail")]
        public async Task<IActionResult> GetOrderDetail()
        {
            var userId = CurrentUserId;
            var result = await _orderService.GetOrdersByUserIdAsync(userId);
            return HandleResult(result);
        }
    }
}