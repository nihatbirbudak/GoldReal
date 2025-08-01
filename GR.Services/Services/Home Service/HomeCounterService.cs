using GR.Core.Interface;
using GR.Models.Entities.Home_Entities;
using GR.Services.Abstract.HomeService;
using GR.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Services.Home_Service
{
    public class HomeCounterService : ServiceBase<HomeCounter, int>, IHomeCounterService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeCounterService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HomeCounter> GetAsync()
        {
            return await _unitOfWork.GetRepository<HomeCounter, int>().GetAsync();
        }
    }
}
