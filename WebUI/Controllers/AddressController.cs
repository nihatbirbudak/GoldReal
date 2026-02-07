using GR.Services.Abstract.PropertyServiceFolder;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : Controller
    {
        private readonly IDistrictService districtService;
        public AddressController(IDistrictService districtService) => this.districtService = districtService;

        // GET /api/address/districts?cityId=41
        [HttpGet("districts")]
        public async Task<IActionResult> GetDistricts([FromQuery] int cityId)
        {
            if (cityId <= 0) return Ok(Array.Empty<object>());
            var list = await districtService.GetDistrictsByCityIdAsync(cityId);
            return Ok(list); // JSON döner
        }

        // GET /api/address/neighborhoods?districtId=480
        [HttpGet("neighborhoods")]
        public async Task<IActionResult> GetNeighborhoods([FromQuery] int districtId, [FromServices] INeighborhoodService nSvc)
        {
            if (districtId <= 0) return Ok(Array.Empty<object>());
            var list = await nSvc.GetNeighborhoodsByDistrictIdAsync(districtId);
            return Ok(list);
        }
    }
}
