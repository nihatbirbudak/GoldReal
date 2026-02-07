using GR.Core.Interface;
using GR.Infrastructure.Repositories;
using GR.Models.DTOs;
using GR.Models.Entities;
using GR.Services.Abstract;
using GR.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Services
{
    public class ContactRequestService : ServiceBase<ContactRequest, int>, IContactRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactRequestService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<ContactRequest>> GetPageAsync(int page, int pageSize)
        {
            var repo = _unitOfWork.GetRepository<ContactRequest, int>();
            // 1) Önce listeyi çek
            var items = (await repo.GetAllAsync(
                orderBy: q => q.OrderByDescending(p => p.CreatedAt),
                skip: Math.Max(0, (page - 1) * pageSize),
                take: pageSize > 0 ? pageSize : 10
            )).ToList();
            // 2) Toplam sayıyı çek
            var totalCount = await repo.CountAsync();
            // 3) Sonucu paketle ve döndür
            return new PagedResult<ContactRequest>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
