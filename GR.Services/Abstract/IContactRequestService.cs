using GR.Core.Interface;
using GR.Models.DTOs;
using GR.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Abstract
{
    public interface IContactRequestService : IServiceBase<ContactRequest,int>
    {
        Task<PagedResult<ContactRequest>> GetPageAsync(int page, int pageSize);
    }
}
