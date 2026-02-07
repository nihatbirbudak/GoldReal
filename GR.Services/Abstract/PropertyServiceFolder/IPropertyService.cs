using GR.Core.Interface;
using GR.Models.DTOs;
using GR.Models.Entities.Property;
using GR.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Abstract.PropertyServiceFolder
{
    public interface IPropertyService : IServiceBase<Property, int>
    {
        Task<List<Property>> GetPropertiesByUserId(string userId);
        Task<PagedResult<PropertyListItemDTO>> GetPageAsync(
    int page, int pageSize, string? ownerId = null);

        Task<PagedResult<PropertyListItemDTO>> GetPageAsync(PropertyListQuery q);
        Task<Property?> GetDetailAsync(int id);
    }
}
