using GR.Models.DTOs;
using GR.Models.Entities.Property;
using GR.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels.PropertyViewModelFolder
{
    public class PropertyListViewModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public PropertyListQuery Query { get; set; } = new();
        public IEnumerable<PropertyListItemDTO> Properties { get; set; } = new List<PropertyListItemDTO>();

        public List<PropertyPhoto>? Photos { get; set; }
    }
}

