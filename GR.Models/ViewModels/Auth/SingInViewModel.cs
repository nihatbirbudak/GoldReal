using System.ComponentModel.DataAnnotations;

namespace GR.Models.ViewModels.Auth
{
    public class SingInViewModel
    {
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "Email :",Prompt = "Email")]
        [DataType(DataType.EmailAddress)]
        
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Şifre :", Prompt = "Şifre")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string Password { get; set; } = null!;

        [Display(Name = "Beni Hatırla ")]
        public bool RememberMe { get; set; }
    }
}
