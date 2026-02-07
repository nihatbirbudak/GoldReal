using GR.Models.Entities.Property;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? description { get; set; }
        public string? PicturePath { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? EmployeeStatus { get; set; }
        public ICollection<Property> Properties { get; set; } = new List<Property>();
        public bool IsActive { get; set; } = true;
    }
}
