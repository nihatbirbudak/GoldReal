using GR.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Abstract.Auth
{
    public interface IAppRoleService
    {
        Task<List<AppRole>> gelAll();
    }
}
