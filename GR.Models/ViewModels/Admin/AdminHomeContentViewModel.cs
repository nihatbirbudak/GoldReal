using GR.Models.Entities;
using GR.Models.Entities.Home_Entities;
using System.Collections.Generic;

namespace GR.Models.ViewModels.Admin
{
    public class AdminHomeContentViewModel
    {
        public IEnumerable<HomeBanner>? Banners { get; set; }
        public IEnumerable<HomeSection>? Sections { get; set; }
        public HomeContact? HomeContact { get; set; }
        public HomeCounter? HomeCounter { get; set; }
        public IEnumerable<CustomerReview>? CustomerReviews { get; set; }
    }
}