using GR.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.Entities.Property
{
    public class PropertyPhoto : Entity<int>
    {

        [Required]
        public int PropertyId { get; set; }

        [ForeignKey(nameof(PropertyId))]
        public Property Property { get; set; } = default!;

        [Required]
        [MaxLength(300)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Caption { get; set; }

        public bool IsCover { get; set; } = false;

        public int? SortOrder { get; set; } = 0;
    }
}
