using GR.Core.Entities.Base;
using GR.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Property
{
    public class Property : Entity<int>
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; } = "TRY";
        public int? GrossM2 { get; set; }
        public int? NetM2 { get; set; }
        public string? RoomPlan { get; set; }
        public string? BathroomCount { get; set; }

        public int CategoryId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? SubtypeId { get; set; }

        public PropertyCategory Category { get; set; } = default!;
        public TransactionType TransactionType { get; set; } = default!;
        public PropertySubtype? Subtype { get; set; }

        // ---- Sahiplik (Owner) ----
        public string OwnerId { get; set; } = default!;  // IdentityUser primary key (string)
        public AppUser Owner { get; set; } = default!;

        // Opsiyonel izleme alanları
        public string ListingNo { get; set; } = Guid.NewGuid().ToString("N");

        // Lokasyon vb.
        public string? Floor { get; set; }
        public string? TotalFloors { get; set; }
        public string? BuildingAge { get; set; }
        public bool? IsFurnished { get; set; }
        public string? Heating { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? NeighborhoodId { get; set; }      // bazı yerlerde mahalle zorunlu olmayabilir
        public string? AddressLine { get; set; }      // serbest metin: "8245/1 Sk. No:14 D:5"
        public string? AddressNote { get; set; }      // kapı tarifi vb.

        public string? homePageImagePath { get; set; } // Anasada gösterilecek resim yolu
        public bool IsActive { get; set; } // yayında mı?
        public bool IsSold { get; set; } // satıldı mı?

        public City City { get; set; } = default!;
        public District District { get; set; } = default!;
        public Neighborhood? Neighborhood { get; set; }
        public ICollection<PropertyPhoto> PropertyPhotos { get; set; } = new HashSet<PropertyPhoto>();
    }
}
