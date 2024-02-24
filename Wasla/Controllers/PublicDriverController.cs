using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Models;
using Wasla.Services.EntitiesServices.PublicDriverServices;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicDriverController : ControllerBase
    {
        private readonly IDriverServices _driverService;
        public PublicDriverController(IDriverServices driverService)
        {
            _driverService = driverService;
        }
        [HttpGet("Packages/{driverId}")]
        public async Task<IActionResult> GetDriverPackagesAsync([FromRoute] string driverId)
        {
            return Ok(await _driverService.GetDriverPublicPackagesAsync(driverId));
        }
        [HttpGet("Packages/request/{driverId}")]
        public async Task<IActionResult> GetPublicPackagesRequestAsync([FromRoute] string driverId)
        {
            return Ok(await _driverService.GetPublicPackagesRequestAsync(driverId));
        }
        [HttpPut("Package/{packageId}/{status}")]
        public async Task<IActionResult> ReviewPackagesRequest(int packageId,int status)
        {
            return Ok(await _driverService.ReviewPackagesRequest(packageId,status));
        }
    }
}
