using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken>
    {
        public RefreshTokenRepository(VTTShopDBContext context) : base(context)
        { 
        }
             public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await base.GetAll().FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<List<RefreshToken>> GetByUserIdAsync(long userId)
        {
            return await base.GetAll().Where(t => t.UserId == userId).ToListAsync();
        }
    }

    
    
}
