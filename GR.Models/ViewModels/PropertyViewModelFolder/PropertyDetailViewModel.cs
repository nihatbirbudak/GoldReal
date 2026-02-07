using GR.Models.DTOs;
using GR.Models.Entities.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels.PropertyViewModelFolder
{
    public sealed class PropertyDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; } = "TRY";
        public int? GrossM2 { get; set; }
        public int? NetM2 { get; set; }
        public string? RoomPlan { get; set; }
        public string? BathroomCount { get; set; }

        public int CategoryId { get; set; }
        public int TransactionTypeId { get; set; }
        public int? SubtypeId { get; set; }
        public string? CategoryName { get; set; }
        public string? TransactionTypeName { get; set; }
        public string? SubtypeName { get; set; }

        public string ListingNo { get; set; } = default!;
        public string? Floor { get; set; }
        public string? TotalFloors { get; set; }
        public string? BuildingAge { get; set; }
        public bool? IsFurnished { get; set; }
        public string? Heating { get; set; }

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? NeighborhoodId { get; set; }
        public string? AddressLine { get; set; }
        public string? AddressNote { get; set; }
        public string? CityName { get; set; }
        public string? DistrictName { get; set; }
        public string? NeighborhoodName { get; set; }

        public bool IsActive { get; set; }
        public bool IsSold { get; set; }

        public List<PropertyPhoto> Photos { get; set; } = new();
        public List<PropertyPhoto> SmilarPhotos { get; set; } = new();
        public IEnumerable<PropertyListItemDTO> Similar { get; set; } = new List<PropertyListItemDTO>();
    }
}
