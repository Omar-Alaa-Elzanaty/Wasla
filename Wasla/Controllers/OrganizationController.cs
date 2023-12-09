using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Wasla.Model.Dtos;
using Wasla.Services.OrganizationSerivces;

namespace Wasla.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize(Roles ="Organization")]
	public class OrganizationController : ControllerBase
	{
		private readonly IOrganizationService _orgService;

		public OrganizationController(IOrganizationService organizationService)
		{
			_orgService = organizationService;
		}
		[HttpPost("vehicle/add")]
		public async Task<IActionResult> AddVehicle([FromForm] VehicleDto model)
		{
			var accId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			return Ok(await _orgService.AddVehicleAsync(model, accId??""));
		}
		[HttpPut("vehicle/update/{vehicleId}")]
		public async Task<IActionResult> UpdateVehicle(int vehicleId,[FromForm]VehicleDto model)
		{

			return Ok(await _orgService.UpdateVehicleAsync(model, vehicleId));
		}
		[HttpDelete("vehicle/delete")]
		public async Task<IActionResult>DeleteVehicle(int vehicleId)
		{
			return Ok(await _orgService.DeleteVehicle(vehicleId));
		}
		[HttpPost("addDriver")]
		public async Task<IActionResult> AddDriver([FromForm]OrgDriverDto model)
		{
			var orgId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			return Ok(await _orgService.AddDriver(model,orgId));
		}
		[HttpGet("vehicle/vehicleAnalysis")]
		public async Task<IActionResult> VehicleAnalysis()
		{
			var accId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			return Ok(await _orgService.VehicleAnalysisAsync(accId??""));
		}
	}
}
