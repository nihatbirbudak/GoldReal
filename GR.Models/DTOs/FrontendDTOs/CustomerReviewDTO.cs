using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.DTOs.FrontendDTOs
{
    public class CustomerReviewDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }   // Kullanıcı adı

        [Required]
        [StringLength(100)]
        public string Surname { get; set; } // Kullanıcı soyadı

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; } // Satış, Kiralama, Yatırım vs.

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }   // Yorum içeriği

        [Range(1, 5)]
        public double Rating { get; set; }       // Yıldız (1–5)
    }
}
