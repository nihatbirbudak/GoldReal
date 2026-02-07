using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GR.Infrastructure.Repositories
{
    public interface IRepository<T, Tid> where T : IEntity<Tid> where Tid : struct
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] includes);
        Task<TResult?> GetProjectedAsync<TResult>(
        Expression<Func<T, bool>> filter,
        Expression<Func<T, TResult>> selector,
        params Expression<Func<T, object>>[] includes);

        Task<IEnumerable<TResult>> GetAllProjectedAsync<TResult>(
            Expression<Func<T, bool>>? filter,
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] includes);

        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    }
}
