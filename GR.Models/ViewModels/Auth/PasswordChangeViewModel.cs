using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels.Auth
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Eski Parola")]
        public string? PasswordOld { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Yeni Parola")]
        public string? PasswordNew { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(PasswordNew), ErrorMessage = "Parola eşleşmedi")]
        [Required(ErrorMessage = "Bu alan boş bırakılamaz")]
        [Display(Name = "Yeni Parola Tekrar")]
        public string? PasswordNewConfirm { get; set; }

        public string? Id { get; set; }
    }
}
