using GR.Core.Entities.Base;
using GR.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Core.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T, TId> GetRepository<T, TId>() where T : class, IEntity<TId> where TId : struct;
        Task<int> SaveChangesAsync();
    }
}
