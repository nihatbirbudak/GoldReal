using GR.Core.Entities.Base;
using GR.Core.Interface;
using GR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }
        private bool _disposed = false;
        public IRepository<T, TId> GetRepository<T, TId>() where T : class, IEntity<TId> where TId : struct
        {
            if (_repositories.TryGetValue(typeof(T), out var repo))
            {
                return (IRepository<T, TId>)repo;
            }

            var repositoryInstance = new RepositoryBase<T, TId>((AppDbContext)_context);
            _repositories.Add(typeof(T), repositoryInstance);
            return repositoryInstance;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
