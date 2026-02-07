using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Property
{
    public class TransactionType : Entity<int>
    {
        public string Name { get; set; } = default!;   // "Satılık", "Kiralık", "Devren Satılık", "Devren Kiralık"
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
