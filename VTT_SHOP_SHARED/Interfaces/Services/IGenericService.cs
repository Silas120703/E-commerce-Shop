using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_SHARED.Interfaces.Services
{
    public interface IGenericService<T> : IService where T : IEntityModel
    {
    }
}
