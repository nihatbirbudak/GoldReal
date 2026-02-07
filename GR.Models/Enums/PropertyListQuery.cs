using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Enums
{
    public enum PropertySortBy { CreatedAt, Price, Title }
    public enum SortDir { Asc, Desc }
    public sealed class PropertyListQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;

        public string? OwnerId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsSold { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? NeighborhoodId { get; set; }
        public int? CategoryId { get; set; }
        public int? TransactionTypeId { get; set; }

        public string? Search { get; set; } // başlık/açıklama/adreste arama

        public PropertySortBy SortBy { get; set; } = PropertySortBy.CreatedAt;
        public SortDir SortDir { get; set; } = SortDir.Desc;
    }
}
