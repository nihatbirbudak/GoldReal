using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Property
{
    public class Neighborhood : Entity<int>
    {
        public int DistrictId { get; set; }
        public string Name { get; set; } = default!;
        public string? NameNom { get; set; }   // "35" gibi (opsiyonel)
        public string? PostalCode { get; set; } // "1" veya "0" (opsiyonel)
        public District District { get; set; } = default!;
    }
}
