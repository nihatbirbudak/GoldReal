using GR.Core.Interface;
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
    public class CustomerReviewService : ServiceBase<CustomerReview, int>, ICustomerReviewService
    {
        public CustomerReviewService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
