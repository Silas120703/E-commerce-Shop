using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class UserRoleRepository : RepositoryBase<UserRole>
    {
        public UserRoleRepository(VTTShopDBContext context) : base(context)
        {
            
        }

        public async Task<UserRole> AddUserRoleAsync(UserRole role)
        {
            return await base.AddAsync(role);
        }

        public UserRole UpdateUserRole(UserRole role)
        {
            return base.Update(role);
        }

        public void DeleteUserRole(UserRole role)
        {
            base.Delete(role);
        }
         
    }
}
