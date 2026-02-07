using GR.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels
{
    public class ContactRequestViewModel
    {
        public IReadOnlyList<ContactRequest>? ContactRequests { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
