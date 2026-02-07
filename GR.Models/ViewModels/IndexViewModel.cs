using GR.Core.Entities.Identity;
using GR.Models.DTOs;
using GR.Models.DTOs.FrontendDTOs;
using GR.Models.DTOs.FrontendDTOs.HomeDTOs;
using GR.Models.Entities;
using GR.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Models.ViewModels
{
    public class IndexViewModel
    {
        public List<HomeBannerFrontendDTO>? Banners;
        public List<HomeSectionFrontendDTO>? Sections;
        public List<PropertyTypeDTO>? PropertyTypes;
        public ContactRequestDTO? ContactRequest;
        public List<RequestType>? RequestTypes;
        public HomeContactDTO? HomeContact;
        public HomeCounterDTO? HomeCounter;
        public List<CustomerReviewDTO>? CustomerReviews;
        public List<AppUser>? Users;
    }
}
