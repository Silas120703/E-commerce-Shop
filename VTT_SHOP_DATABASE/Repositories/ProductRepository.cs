using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Extensions;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class ProductRepository : RepositoryBase<Product>
    {
        public ProductRepository(VTTShopDBContext context) : base(context) 
        { 

        }
        public virtual async Task<Product> AddProductAsync(Product product)
        {
            return await base.AddAsync(product);
        }

        public virtual Product UpdateProduct(Product product)
        {
            return base.Update(product);
        }

        public virtual async Task<Product?> GetProductByIdAsync(long id)
        {
            return await base.GetAll()
                .Include(p => p.ProductPictures)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public virtual async Task<List<Product>> SearchProductByNameAsync(string name)
        {
            return await base.GetAll()
                .Include(p => p.ProductPictures)
                .Where(p => p.Name.Contains(name) || p.Description.Contains(name))
                .ToListAsync();
        }

        public virtual async Task<List<Product>> FilterProductByPriceAsync(decimal priceMin,decimal priceMax)
        {
            return await base.GetAll()
                .Include(p => p.ProductPictures)
                .Where(p => p.Price >= priceMin && p.Price <= priceMax).ToListAsync();
        }
        public virtual bool SoftDeleteProduct(Product product)
        {
            if (product != null)
            {
                product.IsDeleted = true;
                product.DeleteAt = DateTime.Now;
                return true;
            }
            return false;
        }

        public virtual bool HardDeleteProduct(Product product)
        {
            if (product != null)
            {
                base.Delete(product);
                return true;
            }
            return false;
        }

        public virtual Product AddSlugName(Product product)
        {
             product.SlugName = SlugExtensions.ToSlug(product.Name, product.Id);
             base.Update(product);
             return product;
        }
    }
}
