using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Services.Authentication.AdminServices;


namespace Wasla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public SystemAdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet("Organizations")]
        public async Task<IActionResult> GetAllOrgs()
        {
            return Ok(await _adminService.GetAllOrgsAsync());
        }
        [HttpGet("OrganizationRequests")]
        public async Task<IActionResult> GetAllOrganizationRequest()
        {
            return Ok(await _adminService.DisplayOrganizationRequestAsync());
        }
        [HttpPost("organization/account/confirm/{id}")]
        public async Task<IActionResult> ConfrimOrganizationAccount(int id)
        {
            return Ok(await _adminService.ConfirmOrgnaizationRequestAsync(id));
        }
        [HttpPost("station/add")]
        public async Task<IActionResult> AddStation([FromBody] StationDto model)
        {

            return Ok(await _adminService.AddStationAsync(model));
        }
        [HttpPut("station/{stationId}")]
        public async Task<IActionResult> UpdateStation([FromRoute] int stationId, [FromBody] StationDto model) //circle
        {

            return Ok(await _adminService.UpdateStationAsync(model, stationId));
        }
        [HttpGet("stations")]
        public async Task<IActionResult> GetStations()
        {
            return Ok(await _adminService.GetStationsAsync());
        }
        [HttpGet("station/{id}")]
        public async Task<IActionResult> GetStation([FromRoute] int id)
        {
            return Ok(await _adminService.GetStationAsync(id));
        }
        [HttpDelete("station/{id}")]
        public async Task<IActionResult> DeleteStation([FromRoute] int id)
        {
            return Ok(await _adminService.DeleteStationAsync(id));
        }
    }
}
