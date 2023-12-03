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
		[HttpPost("vehicle/addVehicle")]
		public async Task<IActionResult> AddVehicle([FromForm] AddVehicleDto model)
		{
			var accId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			return Ok(await _orgService.AddVehicleAsync(model, accId??""));
		}
		[HttpGet("vehicle/vehicleAnalysis")]
		public async Task<IActionResult> VehicleAnalysis()
		{
			var accId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			return Ok(await _orgService.VehicleAnalysisAsync(accId??""));
		}
		[HttpPost("addDriver")]
		public async Task<IActionResult> AddDriver(OrgDriverDto model)
		{

			return Ok();
		}
	}
}
