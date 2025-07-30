
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
        public string Title { get; set; }             // Banner başlığı
        public string Description { get; set; }       // Kısa açıklama
        public string LinkText { get; set; }          // Buton/metin adı
        public string LinkUrl { get; set; }           // Link yolu
        public string ImagePath { get; set; }         // Görsel yolu

    }
}
