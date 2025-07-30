using GR.Core.Entities.Base;
using GR.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GR.Infrastructure.Repositories
{
    public class RepositoryBase<T, TId> : IRepository<T, TId> where T : class, IEntity<TId> where TId : struct
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Eğer context içinde aynı Id’ye sahip bir entity track ediliyorsa detach eder.
        /// Böylece update/add sırasında tracking hatası alınmaz.
        /// </summary>
        private void DetachIfTracked(T entity)
        {
            var local = _dbSet.Local.FirstOrDefault(e => e.Id.Equals(entity.Id));
            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            DetachIfTracked(entity);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetQueryable(filter, orderBy, includes)
                .Skip(skip ?? 0)
                .Take(take ?? int.MaxValue)
                .ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            return await GetQueryable(filter, orderBy, includes)
                .FirstOrDefaultAsync();
        }

        public Task UpdateAsync(T entity)
        {
            DetachIfTracked(entity);
            _dbSet.Update(entity);
            return _context.SaveChangesAsync();
        }
        private IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }
    }
}
