using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_CORE.Services.AuthService;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ApiControllerBase
    {
        private readonly UserService _admin;

        public AdminsController(UserService admin) 
        {
            _admin = admin;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole(UserRoleDTO role)
        {
            var newRole = await _admin.AddRoleAsync(role);
            if (newRole != null)
            {
                return Ok(newRole);
            }
            return BadRequest("Add role failed");
        }
    }
}
