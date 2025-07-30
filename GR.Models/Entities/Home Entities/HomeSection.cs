using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Home_Entities
{
    public class HomeSection : Entity<int>
    {
        public string Title { get; set; }            // Bölüm başlığı
        public string Description { get; set; }      // Kısa açıklama
        public string LinkText { get; set; }         // Link adı
        public string LinkUrl { get; set; }          // Link yolu
    }
}
