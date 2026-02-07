using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Property
{
    public class PropertyCategory : Entity<int>
    {
        public string Name { get; set; } = default!;   // "Konut", "Arsa", "İşyeri"
        public string? Slug { get; set; }              // opsiyonel: "konut", "arsa"...
        public ICollection<PropertySubtype> Subtypes { get; set; } = new List<PropertySubtype>();
        public ICollection<Property> Properties { get; set; } = new List<Property>();

    }
}
