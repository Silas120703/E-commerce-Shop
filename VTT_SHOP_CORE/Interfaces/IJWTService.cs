using System.Security.Claims;
using VTT_SHOP_DATABASE.Entities;

namespace VTT_SHOP_CORE.Interfaces
{
    public interface IJWTService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        string GeneratePasswordResetToken(User user);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        ClaimsPrincipal? ValidatePasswordResetToken(string token);
    }
}
