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
    public class NeighborhoodService : ServiceBase<Neighborhood, int>, INeighborhoodService
    {
        private readonly IUnitOfWork UnitOfWork;
        public NeighborhoodService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public async Task<List<Neighborhood>> GetNeighborhoodsByDistrictIdAsync(int districtId)
        {
            return (await UnitOfWork.GetRepository<Neighborhood, int>()
                .GetAllAsync(x => x.DistrictId == districtId)).ToList();
        }
    }
}
