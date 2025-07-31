using AutoMapper;
using GR.Models.DTOs.FrontendDTOs.HomeDTOs;
using GR.Models.ViewModels;
using GR.Services.Abstract.HomeService;
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

        public HomeController(ILogger<HomeController> logger, IHomeSectionService sectionService, IHomeBannerService bannerService,IMapper mapper)
        {
            _logger = logger;
            _sectionService = sectionService;
            _bannerService = bannerService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var bannerList = await _bannerService.GetAllAsync();
            var bannerListDTO = _mapper.Map<List<HomeBannerFrontendDTO>>(bannerList);

            var sectionList = await _sectionService.GetAllAsync();
            var sectionListDTO = _mapper.Map<List<HomeSectionFrontendDTO>>(sectionList);

            var model = new IndexViewModel
            {
                Banners = bannerListDTO,
                Sections = sectionListDTO
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
