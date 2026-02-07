using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Property
{
    public class District : Entity<int>
    {
        public int CityId { get; set; }
        public string Name { get; set; } = default!;
        public string? NameNom { get; set; }   // "35" gibi (opsiyonel)
        public string? IsActive { get; set; } // "1" veya "0" (opsiyonel)
        public City City { get; set; } = default!;
        public ICollection<Neighborhood> Neighborhoods { get; set; } = new List<Neighborhood>();
    }
}
