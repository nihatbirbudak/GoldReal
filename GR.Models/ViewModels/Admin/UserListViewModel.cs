using GR.Core.Entities.Identity;
using GR.Models.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels.Admin
{
    public class UserListViewModel
    {
        public List<AppUser>? Users { get; set; }
        public List<UserRole>? userRoleList { get; set; }
    }
}
