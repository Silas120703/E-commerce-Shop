using Microsoft.EntityFrameworkCore;
using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class AddressRepository : RepositoryBase<Address>
    {
        private readonly VTTShopDBContext _context;

        public AddressRepository(VTTShopDBContext context) : base(context)
        {
            _context = context;
        }
        
        public virtual async Task<List<Address>> GetAdressByUserIdAsync(long userId)
        {
            return await base.GetAll().Where(a => a.UserId == userId).ToListAsync();
        }

        public virtual async Task<Address> AddressAsync(Address address)
        {
            return await base.AddAsync(address);
        }

        public virtual Address UpdateAddress(Address address)
        {
            return base.Update(address);
        }

        public virtual void DeleteAddress(Address address)
        {
            base.Delete(address);
        }

        public virtual async Task UnsetDefaultAddressesAsync(long userId)
        {
            var addresses = await base.GetAll().Where(a => a.UserId == userId && a.IsDefault).ToListAsync();
            foreach (var address in addresses)
            {
                address.IsDefault = false;
                base.Update(address);
            }
        }

    }
}
