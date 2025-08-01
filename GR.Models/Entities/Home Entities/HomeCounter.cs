using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Home_Entities
{
    public class HomeCounter : Entity<int>
    {
        public string? Count1 { get; set; }
        public string? Description1 { get; set; }
        public string? Count2 { get; set; }
        public string? Description2 { get; set; }
        public string? Count3 { get; set; }
        public string? Description3 { get; set; }
        public string? Count4 { get; set; }
        public string? Description4 { get; set; }

    }
}
