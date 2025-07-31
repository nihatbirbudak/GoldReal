using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.DTOs.FrontendDTOs.HomeDTOs
{
    public class HomeSectionFrontendDTO
    {
        public string Title { get; set; }            // Bölüm başlığı
        public string Description { get; set; }      // Kısa açıklama
        public string btnText { get; set; }         // Link adı
        public string btnLink { get; set; }          // Link yolu
    }
}
