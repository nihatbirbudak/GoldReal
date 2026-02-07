using GR.Core.Entities.Identity;
using GR.Models.Entities.Property;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels.PropertyViewModelFolder
{
    public class PropertyAddViewModel
    {
        // Seçimler
        [Required(ErrorMessage = "Lütfen başlık giriniz")]
        [MaxLength(200)]
        public string Title { get; set; } = default!;

        [MaxLength(4000)]
        public string? Description { get; set; }

        [Required, Range(0, 999999999)]
        public decimal Price { get; set; }

        [Required, MaxLength(10)]
        public string Currency { get; set; } = "TRY";

        [Required, Range(1, 100000)]
        public int GrossM2 { get; set; }

        [Range(1, 100000)]
        public int? NetM2 { get; set; }

        [Required]
        [MaxLength(20)]
        public string? RoomPlan { get; set; } // "3+1" vb.

        [MaxLength(10)]
        public string? Floor { get; set; } // "5" vb.

        [MaxLength(10)]
        public string? TotalFloor { get; set; } // "12" vb.

        [Range(0, 500)]
        public string? Age { get; set; } // Bina yaşı

        [Required]
        [MaxLength(10)]
        public string? BathroomCount { get; set; } 

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }

        public int? SubtypeId { get; set; }
        public int? LayoutTypeId { get; set; }

        // Adres
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? NeighborhoodId { get; set; }

        [MaxLength(250)]
        public string? AddressLine { get; set; }

        [MaxLength(250)]
        public string? AddressNote { get; set; }
        public string SelectedOwnerId { get; set; } = default!; // Admin’in başkası adına ekleme yapabilmesi için
        public int Id { get; set; }


        public Property? Property { get; set; }
        public List<PropertyCategory>? PropertyCategories { get; set; }
        public List<PropertySubtype>? PropertySubtypes { get; set; }
        public List<TransactionType>? TransactionTypes { get; set; }
        public List<City>? Cities { get; set; }
        public List<District>? Districts { get; set; }
        public List<Neighborhood>? Neighborhoods { get; set; }
        public List<PropertyPhoto>? PropertyPhotos { get; set; }
        public AppUser? Owner { get; set; }
        public List<AppUser>? Users { get; set; }
    }
}
