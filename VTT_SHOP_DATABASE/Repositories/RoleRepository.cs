using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class RoleRepository : RepositoryBase<Role>
    {
        public RoleRepository(VTTShopDBContext context) : base(context)
        {

        }

        public async Task<Role> AddRole(Role role)
        {
            return await base.AddAsync(role);
        }

        public Role UpdateRole(Role role)
        {
            return  base.Update(role);
        }
        
        public async Task<Role?> GetRoleByName(string roleName)
        {
            return await base.GetAll().FirstOrDefaultAsync( r => r.RoleName == roleName);
        }

        public bool SotfDeleteRole(Role role)
        {
            if (role == null)
            {
                return false;
            }
            role.IsDeleted = true;
            role.DeleteAt = DateTime.UtcNow;
            return true;
        }

        public bool HardDeleteRole(Role role)
        {
            if (role == null)
            {
                return false;
            }
            base.Delete(role);
            return true;
        }

    }
}
