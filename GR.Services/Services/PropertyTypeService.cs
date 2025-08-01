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
    public class PropertyTypeService : ServiceBase<PropertyType, int>, IPropertyTypeService
    {
        public PropertyTypeService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
