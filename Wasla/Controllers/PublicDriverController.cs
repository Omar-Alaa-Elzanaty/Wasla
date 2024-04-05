using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Helpers.Enums;
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
        [HttpGet("packages/{driverId}")]
        public async Task<IActionResult> GetDriverPackagesAsync([FromRoute] string driverId)
        {
            return Ok(await _driverService.GetDriverPublicPackagesAsync(driverId));
        }
        [HttpGet("packages/request/{driverId}")]
        public async Task<IActionResult> GetPublicPackagesRequestAsync([FromRoute] string driverId)
        {
            return Ok(await _driverService.GetPublicPackagesRequestAsync(driverId));
        }
        [HttpPut("package/{packageId}/{status}")]
        public async Task<IActionResult> ReviewPackagesRequest(int packageId,int status)
        {
            return Ok(await _driverService.ReviewPackagesRequest(packageId,status));
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile(string userId)
        {
            return Ok(await _driverService.GetProfileAsync(userId));
        }
        [HttpGet("currentTripStatus")]
        public async Task<IActionResult> GetcurrentTripStatus(string userId)
        {
            return Ok(await _driverService.GetTripStatus(userId));
        }
        [HttpPut("UpdateTripStatus")]
        public async Task<IActionResult> UpdateTripStatus(int tripId,PublicTripSatus status)
        {
            return Ok(await _driverService.UpdateTripStatus(tripId, status));
        }
    }
}
