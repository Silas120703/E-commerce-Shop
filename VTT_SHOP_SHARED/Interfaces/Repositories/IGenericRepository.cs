using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_SHARED.Interfaces.Repositories
{
    public interface IGenericRepository<T> : IRepository where T : IEntityModel
    {
        Task<T> AddAsync(T entity);
        T Add(T entity);    
        void Delete(T entity);
        T Update(T entity);
        Task<T> GetByIdAsync(long id);
        T GetById (long id);
        void UpdateRange(IEnumerable<T> entities);
        void DeleteRange(IEnumerable<T> entities);
        IQueryable<T> GetAll();
    }
}
