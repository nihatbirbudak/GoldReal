using GR.Core.Entities.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels.Admin
{
    public class AddUserViewModel
    {
        [Display(Name = "Kullanıcı Adı",Prompt =("Kullanıcı Adı girilmezse sistem tarafında otomatik atanır."))]
        public string? UserName { get; set; }
        [Display(Name = "Ad",Prompt ="Adınız")]
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz.")]
        public string? Name { get; set; }
        [Display(Name = "Soyad",Prompt ="Soyadınız")]
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
        public string? Surname { get; set; }

        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [Display(Name = "Email",Prompt ="Email")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Lütfen Telefon Numaranızı giriniz")]
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        [Display(Name = "Telefon", Prompt = "Telefon numaranız")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Rol alanı boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Rolü")]
        public string? Role { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Şifre")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifre aynı değildir.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz")]
        [Display(Name = "Şifre Tekrar")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olabilir")]
        public string? PasswordConfirm { get; set; }

        [Display(Name = "Kullanıcı Rolleri")]
        public List<AppRole>? appRoles { get; set; }

        public string? FullName { get; set; }
        [Display(Name = "Açıklama", Prompt = "Açıklama")]
        [MaxLength(500, ErrorMessage = "Açıklama alanı en fazla 500 karakter olabilir.")]
        public string? description { get; set; }
        public string? Id { get; set; }

        public IFormFile? PictureFile { get; set; }
        public string? PicturePath { get; set; }
        [Display(Name = "Çalışan Pozisyonu", Prompt = "Çalıştığı Pozisyonu Giriniz. Örn: Danışman")]
        [Required(ErrorMessage = "Bu alanı boş bırakılamaz.")]
        public string? EmployeeStatus { get; set; }
    }
}
