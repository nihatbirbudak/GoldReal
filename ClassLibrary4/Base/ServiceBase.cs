using GR.Core.Entities.Base;
using GR.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Base
{
    public class ServiceBase<T, TId> : IServiceBase<T, TId>
        where T : class, IEntity<TId>
        where TId : struct
    {
        private readonly IUnitOfWork _unitOfWork;
        public ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<T> AddAsync(T entity)
        {
            var added = await _unitOfWork.GetRepository<T, TId>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return added;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            
            var deleted = await _unitOfWork.GetRepository<T, TId>().GetAsync(z => z.Id.Equals(id));
            if (deleted == null)
                return false;

            await _unitOfWork.GetRepository<T, TId>().DeleteAsync(deleted);
            await _unitOfWork.SaveChangesAsync();
            return true;
           
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return _unitOfWork.GetRepository<T, TId>().GetAllAsync();
        }

        public Task<T?> GetByIdAsync(TId id)
        {
            return _unitOfWork.GetRepository<T, TId>().GetAsync(z => z.Id.Equals(id));
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var updated = await _unitOfWork.GetRepository<T, TId>().GetAsync(z=> z.Equals(entity));
            updated= entity;
            await _unitOfWork.GetRepository<T, TId>().UpdateAsync(updated);
            await _unitOfWork.SaveChangesAsync();
            return updated;
        }
    }
}
