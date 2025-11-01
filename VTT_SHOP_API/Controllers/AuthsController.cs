using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_CORE.DTOs;
using VTT_SHOP_CORE.Errors;
using VTT_SHOP_CORE.Services.AuthService;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ApiControllerBase
    {
        private readonly UserService _userService;

        public AuthsController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDTO user)
        {
            var result = await _userService.Register(user);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result = await _userService.Login(login);
            if (result.IsSuccess)
            {
                return Ok(new {Message = result.Value});
            }
            if (result.HasError<AccountNotVerifiedError>())
            {
                return Unauthorized(new { Message = result.Errors.FirstOrDefault()?.Message });
            }
            if (result.HasError<InvalidCredentialsError>())
            {
                return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
            }
            return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });

        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO refresh)
        {
            var result = await _userService.RefreshToken(refresh);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyTokenDTO token)
        {
             var result = await _userService.VerifyTokenFromEmail(token);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(new { Message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpPost("resend-verify-email")]
        public async Task<IActionResult> ResendVerificationEmail(ResendEmail resend)
        {
            var result = await _userService.ResendVerificationEmail(resend);
            if (result.IsSuccess)
            {
                return Ok(new {Message = result.Reasons.FirstOrDefault()?.Message});
            }
            return BadRequest(new {Message = result.Errors.FirstOrDefault()?.Message});
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgot)
        {
            var result = await _userService.ForgotPassword(forgot);
            if (result.IsSuccess)
            {
                return Ok("Verification email sent");
            }
                return NotFound("Does not exist User");        
        }
        [HttpPost("verify-token-forgot-password")]
        public async Task<IActionResult> VerifyTokenForgotPassword(VerifyTokenDTO token)
        {
            var result = await _userService.VerifyTokenForgotPassword(token);
            if (result.IsSuccess)
            {
                return Ok(new {Token = result.Value});
            }
            return BadRequest("Verification token failed");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            var result = await _userService.ResetPassword(resetPassword);
            if(result.IsSuccess)
            {
                return Ok(new {ResetToken = result.Reasons.FirstOrDefault()?.Message});
            }
            return BadRequest(new {Message = result.Errors.FirstOrDefault()?.Message});
            
        }

    }
}
