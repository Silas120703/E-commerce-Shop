using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Extensions;

namespace VTT_SHOP_DATABASE
{
    public class VTTShopDBContext : DbContext
    {
        public VTTShopDBContext() 
        { 
        }

        public VTTShopDBContext(DbContextOptions<VTTShopDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterDatabaseEntities("VTT_SHOP");
           
            modelBuilder.Entity<Product>()
            .HasQueryFilter(p => !p.IsDeleted && !p.IsBlocked);
            //Example for open lock filter 
            //var blockedProducts = await _context.Products
            //.IgnoreQueryFilters()        // Bỏ global filter
            //.Where(p => p.IsBlocked)     // Chỉ lấy sản phẩm bị khóa
            //.ToListAsync();
        }

    }
}
