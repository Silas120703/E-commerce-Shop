using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.Database.EntityBase;
using VTT_SHOP_SHARED.Interfaces.Repositories;

namespace VTT_SHOP_SHARED.Services
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        protected RepositoryBase()
        {
            
        }
        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            if (dbContext != null)
            {
                _dbSet = _dbContext.Set<T>();
            }
        }
        public virtual T Add(T entity)
        {
            entity.CreateAt = DateTime.UtcNow;
            _dbSet.Add(entity);
            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            entity.CreateAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreateAt = DateTime.UtcNow;
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public virtual T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual T Update(T entity)
        {
            entity.UpdateAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return entity;
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdateAt = DateTime.UtcNow;
            }
            _dbSet.UpdateRange(entities);
        }
    }
}