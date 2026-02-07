using GR.Core.Interface;
using GR.Models.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Abstract.PropertyServiceFolder
{
    public interface INeighborhoodService : IServiceBase<Neighborhood, int>
    {
        Task<List<Neighborhood>> GetNeighborhoodsByDistrictIdAsync(int districtId);
    }
}
