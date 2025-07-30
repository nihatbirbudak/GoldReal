using GR.Core.Interface;
using GR.Models.Entities;
using GR.Services.Abstract.HomeService;
using GR.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Home_Service
{
    public class HomeBannerService : ServiceBase<HomeBanner, int>, IHomeBannerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeBannerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
