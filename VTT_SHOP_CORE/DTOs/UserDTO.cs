using System.ComponentModel.DataAnnotations.Schema;

namespace VTT_SHOP_CORE.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string ProfilePicture { get; set; } = null!;
        public DateOnly? Birthday { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
    public class UserCreateDTO
    {
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateOnly? Birthday { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class VerifyTokenDTO
    {
        public required string Token { get; set; } = null!;
    }

    public class ForgotPassword
    {
        public required string Infor { get; set; }
    }

    public class ResetPasswordDto
    {
        public required string ResetToken { get; set; }
        public required string NewPassword { get; set; }
    }

    public class LoginDTO
    {
        public required string Credential { get; set; }
        public required string Password { get; set; }
    }

    public class ResendEmail()
    {
        public required string Email { get; set; }
    }

    public class AuthResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }

    public class RefreshTokenDTO
    {
        public required string RefreshToken { get; set; }
    }
}
