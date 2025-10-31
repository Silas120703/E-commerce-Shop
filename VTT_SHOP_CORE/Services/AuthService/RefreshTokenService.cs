using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_CORE.Services.AuthService
{
    public class RefreshTokenService : ServiceBase<RefreshToken>
    {
        private RefreshTokenRepository _refreshToken;

        public RefreshTokenService(RefreshTokenRepository refreshToken) : base(refreshToken)
        {
            _refreshToken = refreshToken;
        }

        public async Task<string> GenerateRefreshTokenAsync(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                UserId = user.Id,
                ExpiryAt = DateTime.UtcNow.AddDays(7)
            };
            await _refreshToken.AddAsync(refreshToken);
            return refreshToken.Token;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _refreshToken
                .GetAll()
                .FirstOrDefaultAsync(r => r.Token == token);
        }

        public async Task<bool> IsValidTokenAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);
            return refreshToken != null
                && refreshToken.IsActive;
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await GetByTokenAsync(token);
            if (refreshToken == null) return;
            refreshToken.RevokeAt = DateTime.UtcNow;
             _refreshToken.Update(refreshToken);
        }

        public async Task RevokeAllUserTokensAsync(long userId)
        {
            var tokens = await _refreshToken
                .GetAll()
                .Where(t => t.UserId == userId && t.IsActive)
                .ToListAsync();

            foreach (var t in tokens)
                t.RevokeAt = DateTime.UtcNow;

            _refreshToken.UpdateRange(tokens);
        }

    }
}
