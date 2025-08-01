using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Home_Entities
{
    public class HomeContact : Entity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Title2 { get; set; }
        public string Description2 { get; set; }
        public string title3 { get; set; }
        public string Description3 { get; set; }
    }
}
