using AutoMapper;
using GR.Models.DTOs;
using GR.Models.DTOs.FrontendDTOs;
using GR.Models.DTOs.FrontendDTOs.HomeDTOs;
using GR.Models.Entities;
using GR.Models.Entities.Home_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GR.Services.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Add your AutoMapper configurations here 
            // Example: CreateMap<Source, Destination>();
            // CreateMap<Project, ProjectDto>().ReverseMap();
            // CreateMap<User, UserDto>().ReverseMap();
            CreateMap<HomeBanner, HomeBannerFrontendDTO>().ReverseMap();
            CreateMap<HomeSection, HomeSectionFrontendDTO>().ReverseMap();
            CreateMap<ContactRequest, ContactRequestDTO>().ReverseMap();
            CreateMap<PropertyType, PropertyTypeDTO>().ReverseMap();
            CreateMap<HomeContact, HomeContactDTO>().ReverseMap();
            CreateMap<HomeCounter, HomeCounterDTO>().ReverseMap();
            CreateMap<CustomerReview, CustomerReviewDTO>().ReverseMap();
        }
    }
    
    
}
