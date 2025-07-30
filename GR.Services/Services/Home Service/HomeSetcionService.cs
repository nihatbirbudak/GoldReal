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
    public class HomeSetcionService : ServiceBase<HomeSection, int>, IHomeSectionService
    {
        public HomeSetcionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
