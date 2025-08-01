using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Enum
{
    public enum RequestType
    {
        [Display(Name = "Satın Almak İstiyorum")]
        Buy = 1,    // Satın almak
        [Display(Name = "Satmak İstiyorum")]
        Sell = 2,   // Satmak
        [Display(Name = "Kiralamak İstiyorum")]
        Rent = 3,   // Kiralamak
        [Display(Name = "Diğer")]
        Other = 4   // Diğer
    }
}
