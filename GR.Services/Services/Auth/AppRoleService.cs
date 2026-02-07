using GR.Core.Entities.Identity;
using GR.Services.Abstract.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Services.Auth
{
    public class AppRoleService : IAppRoleService
    {
        private readonly RoleManager<AppRole> roleManager;

        public AppRoleService(RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateAsync(string name)
        {
            return await roleManager.CreateAsync(new AppRole() { Name = name });
        }

        public async Task<List<AppRole>> gelAll()
        {
            return await roleManager.Roles.ToListAsync();
        }
    }
}
