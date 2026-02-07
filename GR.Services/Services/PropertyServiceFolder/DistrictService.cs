using GR.Core.Interface;
using GR.Models.Entities.Property;
using GR.Services.Abstract.PropertyServiceFolder;
using GR.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Services.PropertyServiceFolder
{
    public class DistrictService : ServiceBase<District, int>, IDistrictService
    {
        private readonly IUnitOfWork unitOfWork;
        public DistrictService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<District>> GetDistrictsByCityIdAsync(int cityId)
        {
            return (await unitOfWork.GetRepository<District, int>()
                .GetAllAsync(x => x.CityId == cityId)).ToList();
        }
    }
}
