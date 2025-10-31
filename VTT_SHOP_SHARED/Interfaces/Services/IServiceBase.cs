using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_SHARED.Interfaces.Services
{
    public interface IServiceBase<T>:IGenericService<T> where T : IEntityModel
    {
    }
}
