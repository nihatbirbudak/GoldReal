using GR.Core.Interface;
using GR.Models.Entities.Home_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Abstract.HomeService
{
    public interface IHomeContactService : IServiceBase<HomeContact,int>
    {
        Task<HomeContact> GetAsync();
    }
}
