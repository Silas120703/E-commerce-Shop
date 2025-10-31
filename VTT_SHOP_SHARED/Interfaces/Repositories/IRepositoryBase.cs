using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_SHARED.Interfaces.Repositories
{
    public interface IRepositoryBase<T> :IGenericRepository<T> where T : IEntityModel
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(long id);
        Task<T> AddAsync(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
