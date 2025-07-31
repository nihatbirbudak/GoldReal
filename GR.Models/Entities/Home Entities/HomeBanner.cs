
using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities
{
    public class HomeBanner : Entity<int>
    {
        public string Title { get; set; }            // Bölüm başlığı
        public string Description { get; set; }      // Kısa açıklama
        public string btnText { get; set; }         // Link adı
        public string btnLink { get; set; }          // Link yolu
        public string ImageUrl { get; set; }        // Resim URL'si

    }
}
