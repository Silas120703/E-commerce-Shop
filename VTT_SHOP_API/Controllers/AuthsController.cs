using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VTT_SHOP_SHARED.DTOs;
using VTT_SHOP_CORE.Errors;
using VTT_SHOP_CORE.Services.AuthService;

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
            return HandleResult(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result = await _userService.Login(login);
            return HandleResult(result);

        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDTO refresh)
        {
            var result = await _userService.RefreshToken(refresh);
            return HandleResult(result);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyTokenDTO token)
        {
            var result = await _userService.VerifyTokenFromEmail(token);
            return HandleResult(result);
        }

        [HttpPost("resend-verify-email")]
        public async Task<IActionResult> ResendVerificationEmail(ResendEmail resend)
        {
            var result = await _userService.ResendVerificationEmail(resend);
            return HandleResult(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgot)
        {
            var result = await _userService.ForgotPassword(forgot);
            return HandleResult(result);
        }
        [HttpPost("verify-token-forgot-password")]
        public async Task<IActionResult> VerifyTokenForgotPassword(VerifyTokenDTO token)
        {
            var result = await _userService.VerifyTokenForgotPassword(token);
            return HandleResult(result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            var result = await _userService.ResetPassword(resetPassword);
            return HandleResult(result);

        }

    }
}
