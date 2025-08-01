using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities
{
    public class PropertyType : Entity<int>
    {
        public string Name { get; set; }      // Örn: Villa, Daire, Arsa
        public string? Description { get; set; } // İsteğe bağlı açıklama

        // İlişki (1 tip birden fazla ContactRequest'te kullanılabilir)
        public ICollection<ContactRequest> ContactRequests { get; set; }
    }
}
