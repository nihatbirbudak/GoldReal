using AutoMapper;
using GR.Models.DTOs;
using GR.Models.DTOs.FrontendDTOs;
using GR.Models.DTOs.FrontendDTOs.HomeDTOs;
using GR.Models.Entities;
using GR.Models.Entities.Property;
using GR.Models.Enum;
using GR.Models.Enums;
using GR.Models.ViewModels;
using GR.Models.ViewModels.PropertyViewModelFolder;
using GR.Services.Abstract;
using GR.Services.Abstract.Auth;
using GR.Services.Abstract.HomeService;
using GR.Services.Abstract.PropertyServiceFolder;
using GR.Services.Services;
using GR.Services.Services.PropertyServiceFolder;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAppUserService _appUserService;
        private readonly IPropertyService propertyService;
        private readonly IPropertyPhotosService propertyPhotosService;

        public HomeController(ILogger<HomeController> logger, 
            IHomeSectionService sectionService,
            IHomeBannerService bannerService, IMapper mapper,
            IPropertyTypeService propertyTypeService,
            IContactRequestService contactRequestService,
            IHomeContactService homeContactService,
            ICustomerReviewService customerReviewService, 
            IHomeCounterService homeCounterService,
            IAppUserService appUserService,
            IPropertyService propertyService,
            IPropertyPhotosService propertyPhotosService)
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
            _appUserService = appUserService;
            this.propertyService = propertyService;
            this.propertyPhotosService = propertyPhotosService;
        }
        [AllowAnonymous]
        [Route("")]
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

            var users = await _appUserService.GetUserInIsAvtiveClass();

            var model = new IndexViewModel
            {
                Banners = bannerListDTO,
                Sections = sectionListDTO,
                PropertyTypes = propertyTypeDTO,
                RequestTypes = Enum.GetValues(typeof(RequestType))
                    .Cast<RequestType>().ToList(),
                HomeContact = homeContactDTO,
                HomeCounter = homeCounterDTO,
                CustomerReviews = customerRewviewsDTO,
                Users = users

            };

            return View(model);
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("hakkimizda")]
        public IActionResult About()
        {
            return View();
        }

        [Route("danismanlarimiz")]
        public async Task<IActionResult> AgentList()
        {
            var model = new AgentListViewModel();
            model.appUsers = await _appUserService.getAll();
            return View(model);
        }

        [Route("iletisim")]
        public async Task<IActionResult> Contact()
        {
            var propertyTypes = await _propertyTypeService.GetAllAsync();
            var propertyTypesDTO = _mapper.Map<List<PropertyTypeDTO>>(propertyTypes);

            var model = new ContactViewModel
            {
                PropertyTypes = propertyTypesDTO,
                RequestTypes = Enum.GetValues(typeof(RequestType))
                    .Cast<RequestType>().ToList(),
                ContactRequest = new ContactRequestDTO()
            };
            return View(model);
        }
        [HttpPost]
        [Route("iletisim")]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            // DTO üzerinde DataAnnotations zaten var (Required, Email, Phone vs.)
            if (!ModelState.IsValid)
            {
                var propertyTypes = await _propertyTypeService.GetAllAsync();
                var propertyTypesDTO = _mapper.Map<List<PropertyTypeDTO>>(propertyTypes);
                // Hatalý post’ta dropdown’larý yeniden doldurup ayný view’a dön
                var vm = new ContactViewModel
                {
                    ContactRequest = model.ContactRequest,
                    PropertyTypes = propertyTypesDTO,
                    RequestTypes = Enum.GetValues(typeof(RequestType))
                    .Cast<RequestType>().ToList(),
                };
                return View(vm);
            }

            // Kaydet (entity adlarýný projene göre uyarlayýn)
            var entity = new ContactRequest
            {
                Name = model.Name.Trim(),
                Surname = model.Surname!.Trim(),
                Phone = model.Phone.Trim(),
                Email = model.Email.Trim(),
                Message = model.Message!.Trim(),
                PropertyTypeId = model.PropertyTypeId,
                RequestType = model.RequestType,
                CreatedAt = DateTime.UtcNow
            };

            await _contactRequestService.AddAsync(entity);

            TempData["Success"] = "Mesajýnýz alýndý. En kýsa sürede size dönüþ yapacaðýz.";
            // PRG deseni: yenilemede yeniden post olmasýn
            return RedirectToAction(nameof(Contact));
        }

        [Route("projelerimiz/satista-olan")]
        public async Task<IActionResult> Properties([FromQuery] PropertyListQuery q)
        {
            // Ýlk giriþte varsayýlan: Aktif + Satýlmamýþ (satýþta) + Yeni->Eski
            if (!Request.QueryString.HasValue)
            {
                q.IsSold = false;   // artýk TransactionTypeId yerine IsSold kullanýlýyor
                q.IsActive = true;
                q.SortBy = PropertySortBy.CreatedAt;
                q.SortDir = SortDir.Desc;
                q.Page = q.Page <= 0 ? 1 : q.Page;
                q.PageSize = q.PageSize <= 0 ? 12 : q.PageSize;
            }

            var result = await propertyService.GetPageAsync(q);

            var vm = new PropertyListViewModel
            {
                Properties = result.Items,
                CurrentPage = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,   // PagedResult içinde varsa bunu kullan
                Query = q
            };

            // Fotoðraflarý sayfaya göre getir
            vm.Photos = await propertyPhotosService.GetCurrentCovers(q.Page, q.PageSize);

            return View("Properties", vm);
        }

        [HttpGet]
        [Route("projelerimiz/satista-olan/{id:int}")]
        public async Task<IActionResult> PropertyDetails(int id)
        {
            var p = await propertyService.GetDetailAsync(id);
            if (p == null) return NotFound();

            var orderedPhotos = (p.PropertyPhotos ?? Enumerable.Empty<PropertyPhoto>())
                .OrderByDescending(x => x.IsCover)
                .ThenBy(x => x.SortOrder ?? int.MaxValue)
                .ThenBy(x => x.Id)
                .ToList();

            var similar = await propertyService.GetPageAsync(new PropertyListQuery
            {
                Page = 1,
                PageSize = 2,
                CategoryId = p.CategoryId,
                CityId = p.CityId,
                TransactionTypeId = p.TransactionTypeId,
                IsActive = true,
                IsSold = false,
                SortBy = PropertySortBy.CreatedAt,
                SortDir = SortDir.Desc
            });

            var smilarPhotos = await propertyPhotosService.GetCurrentCovers(1, 2);

            var vm = new PropertyDetailViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Currency = string.IsNullOrWhiteSpace(p.Currency) ? "TRY" : p.Currency,
                GrossM2 = p.GrossM2,
                NetM2 = p.NetM2,
                RoomPlan = p.RoomPlan,
                BathroomCount = p.BathroomCount,

                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                TransactionTypeId = p.TransactionTypeId,
                TransactionTypeName = p.TransactionType?.Name,
                SubtypeId = p.SubtypeId,
                SubtypeName = p.Subtype?.Name,

                ListingNo = p.ListingNo,
                Floor = p.Floor,
                TotalFloors = p.TotalFloors,
                BuildingAge = p.BuildingAge,
                IsFurnished = p.IsFurnished,
                Heating = p.Heating,

                CityId = p.CityId,
                DistrictId = p.DistrictId,
                NeighborhoodId = p.NeighborhoodId,
                CityName = p.City?.Name,
                DistrictName = p.District?.Name,
                NeighborhoodName = p.Neighborhood?.Name,
                AddressLine = p.AddressLine,
                AddressNote = p.AddressNote,

                IsActive = p.IsActive,
                IsSold = p.IsSold,

                Photos = orderedPhotos,
                Similar = similar.Items,
                SmilarPhotos = smilarPhotos
            };

            return View("PropertyDetails", vm);
        }

        [Route("projelerimiz/sattigimiz")]
        public async Task<IActionResult> PropertiesSold([FromQuery] PropertyListQuery q)
        {
            // Ýlk giriþte varsayýlan: Aktif + Satýlmýþ + Yeni->Eski
            if (!Request.QueryString.HasValue)
            {
                q.IsSold = true;   // satýlmýþ ilanlar için IsSold = true
                q.IsActive = true;
                q.SortBy = PropertySortBy.CreatedAt;
                q.SortDir = SortDir.Desc;
                q.Page = q.Page <= 0 ? 1 : q.Page;
                q.PageSize = q.PageSize <= 0 ? 12 : q.PageSize;
            }


            var result = await propertyService.GetPageAsync(q);

            var vm = new PropertyListViewModel
            {
                Properties = result.Items,
                CurrentPage = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
                Query = q
            };

            vm.Photos = await propertyPhotosService.GetCurrentCovers(q.Page, q.PageSize);

            return View("Properties", vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
