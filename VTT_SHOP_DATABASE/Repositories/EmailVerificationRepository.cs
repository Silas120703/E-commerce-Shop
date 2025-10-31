using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class EmailVerificationRepository : RepositoryBase<EmailVerificationToken>
    {
        public EmailVerificationRepository(VTTShopDBContext context) : base(context)
        {
        }

        public async Task<EmailVerificationToken?> GetValidTokenAsync(string token)
        {
            return await GetAll()
                .Include(t => t.User)
                .FirstOrDefaultAsync(t =>
                t.Token == token &&
                !t.IsUsed &&
                t.ExpiredAt > DateTime.UtcNow);
        }

       
    }
}
