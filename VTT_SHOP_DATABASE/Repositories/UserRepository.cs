using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(VTTShopDBContext context) : base(context)
        {

        }
        
        public async Task<User> AddUserAsync(User user)
        {
            return await base.AddAsync(user);
        }

        public User UpdateUser(User  user)
        {
            return base.Update(user);
        }

        public async Task<User?> GetUserByIdAsync(long id)
        {
            return await base.GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> GetUserByPhone(string phone)
        {
            return await base.GetAll()
                .Include(g => g.EmailVerificationToken)
                .FirstOrDefaultAsync(g => g.Phone == phone);
        }

        public async Task<User?> GetUserByPhoneOrEmail(string infor)
        {
            return await base.GetAll()
                .Include(g => g.EmailVerificationToken)
                .FirstOrDefaultAsync(g => g.Phone == infor||g.Email==infor);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await base.GetAll()
                .Include(g => g.EmailVerificationToken)
                .FirstOrDefaultAsync(g => g.Email == email);
        }

        public async Task<List<User>> SearchUser(string search)
        {
            return await base.GetAll().Where(s => s.Phone.Contains(search) || s.Email.Contains(search) || s.Name.Contains(search)).ToListAsync();
        }
    }
}
