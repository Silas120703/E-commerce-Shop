using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.Database.EntityBase;
using VTT_SHOP_SHARED.Interfaces.Repositories;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class Repository<T> : RepositoryBase<T> where T : EntityBase
    {
        public Repository(VTTShopDBContext dbContext) : base(dbContext)
        {
        }
    }
}
