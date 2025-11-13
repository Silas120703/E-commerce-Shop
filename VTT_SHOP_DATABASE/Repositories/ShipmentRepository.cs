using VTT_SHOP_DATABASE.Entities;
using VTT_SHOP_SHARED.Services;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class ShipmentRepository : RepositoryBase<Shipment>
    {
        public ShipmentRepository(VTTShopDBContext context) : base(context)
        {
        }
    }
}