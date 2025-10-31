using VTT_SHOP_SHARED.Database.EntityBase;
using VTT_SHOP_SHARED.Interfaces.Services;

namespace VTT_SHOP_SHARED.Services
{
    public abstract class ServiceBase<T> : IServiceBase<T> where T : EntityBase
    {
        private readonly RepositoryBase<T> _repository;
        protected ServiceBase(RepositoryBase<T> repository)
        {
            _repository = repository;
        }
        public T Add(T entity)
        {
            return _repository.Add(entity);
        }

        public Task<T> AddAsync(T entity)
        {
            return _repository.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _repository.DeleteRange(entities);
        }

        public IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T GetById(long id)
        {
            return _repository.GetById(id);
        }

        public Task<T> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }

        public T Update(T entity)
        {
            return _repository.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _repository.UpdateRange(entities);
        }
    }
}
