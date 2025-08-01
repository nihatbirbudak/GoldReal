using GR.Core.Entities.Base;
using GR.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities
{
    public class ContactRequest : Entity<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public RequestType RequestType { get; set; } // Enum: SatınAlmak/Satmak
        
        // Foreign Key
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyType { get; set; }

        public string Message { get; set; }
    }
}
