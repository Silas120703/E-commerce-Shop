using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class ProductPictureRepository : RepositoryBase<ProductPicture>
    {
        public ProductPictureRepository(VTTShopDBContext context) : base(context)
        {
        }
        public async Task<List<ProductPicture>> GetPicturesByProductIdAsync(long productId)
        {
            return await GetAll().Where(pp => pp.ProductId == productId).ToListAsync();
        }
        public async Task<ProductPicture?> GetMainPictureByProductIdAsync(long productId)
        {
            return await GetAll().FirstOrDefaultAsync(pp => pp.ProductId == productId && pp.IsMain);
        }
    }
}
