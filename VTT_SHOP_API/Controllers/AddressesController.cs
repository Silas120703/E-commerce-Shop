using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.Services.AuthService;
using VTT_SHOP_SHARED.DTOs;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ApiControllerBase
    {
        private readonly AddressService _addressService;

        public AddressesController(AddressService addressService )
        {
            _addressService = addressService;
        }
        [HttpGet("get-addresses-by-user-id")]
        public async Task<IActionResult> GetAddressesByUserId()
        {
            var userId = CurrentUserId;
            var result = await _addressService.GetAddressByUserIdAsync(userId);
            return HandleResult(result);
        }
        [HttpGet("get-address-by-id/{id}")]
        public async Task<IActionResult> GetAddressById(long id)
        {
            var result = await _addressService.GetAddressById(id);
            return HandleResult(result);
        }
        [HttpPost("add-address")]
        public async Task<IActionResult> AddAddress([FromBody] AddressDTO addressDTO)
        {
            var result = await _addressService.AddAddressAsync(addressDTO, CurrentUserId);
            return HandleResult(result);
        }

        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateAddress(AddressDTO addressDTO)
        {
            var result = await _addressService.UpdateAddress(addressDTO);
            return HandleResult(result);
        }

        [HttpDelete("delete-address/{id}")]
        public async Task<IActionResult> DeleteAddress(long id)
        {
            var result = await _addressService.DeleteAddress(id);
            return HandleResult(result);
        }

    }
}
