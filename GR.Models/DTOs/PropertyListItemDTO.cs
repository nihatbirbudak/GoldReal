using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.DTOs
{
    public sealed class PropertyListItemDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? AddressLine { get; set; }
        public string CityName { get; set; } = "";
        public string DistrictName { get; set; } = "";
        public string NeighborhoodName { get; set; } = "";
        public decimal Price { get; set; }
        public string Currency { get; set; } = "TRY";
        public string TransactionTypeName { get; set; } = "";
        public string HomePageImagePath { get; set; } = "";
        public bool IsActive { get; set; }
        public string ownerFullName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public bool IsSold { get; set; }
        public int? GrossM2 { get; set; }
    }
}
