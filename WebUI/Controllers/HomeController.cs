using AutoMapper;
using GR.Models.DTOs;
using GR.Models.DTOs.FrontendDTOs;
using GR.Models.DTOs.FrontendDTOs.HomeDTOs;
using GR.Models.Enum;
using GR.Models.ViewModels;
using GR.Services.Abstract;
using GR.Services.Abstract.HomeService;
using GR.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeBannerService _bannerService;
        private readonly IHomeSectionService _sectionService;
        private readonly IMapper _mapper;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IContactRequestService _contactRequestService;
        private readonly IHomeContactService _homeContactService;
        private readonly ICustomerReviewService _customerReviewService;
        private readonly IHomeCounterService _homeCounterService;

        public HomeController(ILogger<HomeController> logger, 
            IHomeSectionService sectionService,
            IHomeBannerService bannerService, IMapper mapper,
            IPropertyTypeService propertyTypeService,
            IContactRequestService contactRequestService,
            IHomeContactService homeContactService,
            ICustomerReviewService customerReviewService, 
            IHomeCounterService homeCounterService)
        {
            _logger = logger;
            _sectionService = sectionService;
            _bannerService = bannerService;
            _mapper = mapper;
            _propertyTypeService = propertyTypeService;
            _contactRequestService = contactRequestService;
            _homeContactService = homeContactService;
            _customerReviewService = customerReviewService;
            _homeCounterService = homeCounterService;
        }

        public async Task<IActionResult> Index()
        {
            var bannerList = await _bannerService.GetAllAsync();
            var bannerListDTO = _mapper.Map<List<HomeBannerFrontendDTO>>(bannerList);

            var sectionList = await _sectionService.GetAllAsync();
            var sectionListDTO = _mapper.Map<List<HomeSectionFrontendDTO>>(sectionList);

            var propertyType = await _propertyTypeService.GetAllAsync();
            var propertyTypeDTO = _mapper.Map<List<PropertyTypeDTO>>(propertyType);

            var homeContact = await _homeContactService.GetAsync();
            var homeContactDTO = _mapper.Map<HomeContactDTO>(homeContact);

            var homeCounter = await _homeCounterService.GetAsync();
            var homeCounterDTO = _mapper.Map<HomeCounterDTO>(homeCounter);

            var customerRewviews = await _customerReviewService.GetAllAsync();
            var customerRewviewsDTO = _mapper.Map<List<CustomerReviewDTO>>(customerRewviews);



            var model = new IndexViewModel
            {
                Banners = bannerListDTO,
                Sections = sectionListDTO,
                PropertyTypes = propertyTypeDTO,
                RequestTypes = Enum.GetValues(typeof(RequestType))
                    .Cast<RequestType>().ToList(),
                HomeContact = homeContactDTO,
                HomeCounter = homeCounterDTO,
                CustomerReviews = customerRewviewsDTO
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
