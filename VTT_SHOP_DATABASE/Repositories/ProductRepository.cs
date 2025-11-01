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
        public async Task<Product> AddProductAsync(Product product)
        {
            return await base.AddAsync(product);
        }

        public Product UpdateProduct(Product product)
        {
            return base.Update(product);
        }

        public async Task<Product> GetProductByIdAsync(long id)
        {
            return await base.GetByIdAsync(id);
        }

        public async Task<List<Product>> SearchProductByNameAsync(string name)
        {
            return await base.GetAll()
                             .Where(p => p.Name.Contains(name) || p.Description.Contains(name))
                             .ToListAsync();
        }

        public async Task<List<Product>> FilterProductByPriceAsync(decimal priceMin,decimal priceMax)
        {
            return await base.GetAll().Where(p => p.Price >= priceMin && p.Price <= priceMax).ToListAsync();
        }
        public bool SoftDeleteProduct(Product product)
        {
            if (product != null)
            {
                product.IsDeleted = true;
                product.DeleteAt = DateTime.Now;
                return true;
            }
            return false;
        }

        public bool HardDeleteProduct(Product product)
        {
            if (product != null)
            {
                base.Delete(product);
                return true;
            }
            return false;
        }

        public Product AddSlugName(Product product)
        {
             product.SlugName = SlugExtensions.ToSlug(product.Name, product.Id);
             base.Update(product);
             return product;
        }
    }
}
