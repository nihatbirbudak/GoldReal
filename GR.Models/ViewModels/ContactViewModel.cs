using GR.Models.DTOs;
using GR.Models.Entities;
using GR.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels
{
    public class ContactViewModel
    {
        public ContactRequestDTO? ContactRequest = new();
        public List<PropertyTypeDTO>? PropertyTypes = new();
        public List<RequestType>? RequestTypes = new();

        public int Id { get; set; }
        [Display(Name = "Ad", Prompt = "Ad")]
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string Name { get; set; }

        [Display(Name = "Soyad", Prompt = "Soyad")]
        public string? Surname { get; set; }

        [Display(Name = "Telefon", Prompt = "Telefon")]
        [Required(ErrorMessage = "Telefon alanı zorunludur.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string Phone { get; set; }

        [Display(Name = "E-posta", Prompt = "E-posta")]
        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Display(Name = "Mesajınızı buraya yazabilirsiniz.", Prompt = "Mesajınızı buraya yazabilirsiniz.")]
        public string? Message { get; set; }

        [Display(Name = "İlgilendiğiniz tür")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public int PropertyTypeId { get; set; } // İlişkili PropertyType'ın ID'si

        [Display(Name = "Satınlamak istiyorum / Satmak İstiyorum", Prompt = "Lütfen Seçiniz")]
        [Required(ErrorMessage = "Bu alan zorunludur.")]
        public RequestType RequestType { get; set; }
    }
}
