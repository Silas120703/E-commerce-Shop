using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;

namespace VTT_SHOP_DATABASE.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VTTShopDBContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(VTTShopDBContext context)
        {
            _context = context;
        }
        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction?.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                 _transaction.DisposeAsync();
            }
            _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public async Task DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public async Task RollbackAsync()
        {
            try
            {
                await _transaction?.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }




        
    }
}
