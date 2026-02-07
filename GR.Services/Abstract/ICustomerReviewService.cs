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
    public interface ICustomerReviewService : IServiceBase<CustomerReview,int>
    {
        Task<PagedResult<CustomerReview>> GetPageAsync(int page, int pageSize);
    }
}
