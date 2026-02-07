using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Property
{
    public class PropertySubtype : Entity<int>
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = default!;   // "Daire", "Rezidans", "Müstakil Ev"...

        public PropertyCategory Category { get; set; } = default!;
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
