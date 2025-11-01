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

        public RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public T Add(T entity)
        {
            entity.CreateAt = DateTime.UtcNow;
            _dbSet.Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.CreateAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreateAt = DateTime.UtcNow;
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Update(T entity)
        {
            entity.UpdateAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            return entity;
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdateAt = DateTime.UtcNow;
            }
            _dbSet.UpdateRange(entities);
        }
    }
}