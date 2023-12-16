using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Wasla.Model.Dtos;
using Wasla.Model.Models;
using Wasla.Services.OrganizationSerivces;
using Wasla.Services.Exceptions.FilterException;

namespace Wasla.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize(Roles ="Organization")]
	public class OrganizationController : ControllerBase
	{
		private readonly IOrganizationService _orgService;
		private readonly UserManager<Account> _userManager;

		public OrganizationController(IOrganizationService organizationService, UserManager<Account> userManager)
		{
			_orgService = organizationService;
			_userManager = userManager;
		}
		[HttpGet("{orgId}/vehicles")]
		public async Task<IActionResult> DisplayVehicles(string orgId)
		{
			return Ok(await _orgService.DisplayVehicles(orgId));
		}
		[HttpPost("{orgId}/vehicle/add")]
		public async Task<IActionResult> AddVehicle([FromForm] VehicleDto model,string orgId)
		{ 
			return Ok(await _orgService.AddVehicleAsync(model, orgId));
		}
		[HttpPut("vehicle/update/{vehicleId}")]
		public async Task<IActionResult> UpdateVehicle(int vehicleId,[FromForm]VehicleDto model)
		{

			return Ok(await _orgService.UpdateVehicleAsync(model, vehicleId));
		}
		[HttpDelete("vehicle/delete/{vehicleId}")]
		public async Task<IActionResult>DeleteVehicle(int vehicleId)
		{
			return Ok(await _orgService.DeleteVehicleAsync(vehicleId));
		}
		[HttpPost("{orgId}/driver/add")]
		public async Task<IActionResult> AddDriver([FromForm]OrgDriverDto model,string orgId)
		{
			return Ok(await _orgService.AddDriverAsync(model,orgId));
		}
		[HttpGet("{orgId}/vehicle/vehicle-analysis")]
		public async Task<IActionResult> VehicleAnalysis(string orgId)
		{
			return Ok(await _orgService.VehicleAnalysisAsync(orgId??""));
		}
		[HttpPost("{orgId}/employee/add")]
		public async Task<IActionResult> AddEmployee([FromForm]EmployeeRegisterDto model,string orgId)
		{
			return Ok(await _orgService.AddEmployeeAsync(model,orgId));
		}
		[HttpDelete("employee/delete/{empId}")]
		public async Task<IActionResult>DeleteEmployee(string empId)
		{
			return Ok(await _orgService.DeleteEmployeeAsync(empId));
		}
		[HttpGet("{orgId}/drviers")]
		public async Task<IActionResult> GetAllDrivers(string orgId)
		{
			return Ok(await _orgService.GetAllDrivers(orgId));
		}
		#region Ads
		[HttpPost("{orgId}/ads/add")]
		public async Task<IActionResult> AddAds([FromForm]AdsDto model, string orgId)
		{
			return Ok(await _orgService.AddAdsAsync(model, orgId));
		}
		[HttpPost("vehicle/{vehicleId}/ads/add/{adsId}")]
		public async Task<IActionResult> AddVehicleAds(int adsId, int vehicleId)
		{
			return Ok(await _orgService.AddAdsToVehicleAsync(adsId, vehicleId));
		}
		[HttpDelete("vehicle/{vehicleId}/ads/remove/{adsId}")]
		public async Task<IActionResult> RemoveVehicleAds(int adsId, int vehicleId)
		{
			return Ok(await _orgService.RemoveAdsFromVehicleAsync(adsId, vehicleId));
		}
		[HttpPut("ads/update/{adsId}")]
		public async Task<IActionResult> UpdateAds(int adsId,[FromForm] AdsDto model)
		{
			return Ok(await _orgService.UpdateAdsAsync(adsId, model));
		}
		[HttpDelete("ads/delete/adsId")]
		public async Task<IActionResult> DeleteAds(int adsId)
		{
			return Ok(await _orgService.DeleteAdsAsync(adsId));
		}
		#endregion
		[HttpPost("employee/add")]
		public async Task<IActionResult> AddEmployee([FromForm] EmployeeRegisterDto model)
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			string? orgId = null;
			if (userName is not null)
			{
				orgId = _userManager.FindByNameAsync(userName).Result?.Id;
			}

			return Ok(await _orgService.AddEmployeeAsync(model, orgId));
		}
		[HttpPost("station/add/{orgId}")]
		public async Task<IActionResult> AddStation([FromRoute] string orgId, [FromBody] StationDto model)
		{

			return Ok(await _orgService.AddStationAsync(model, orgId));
		}
		[HttpPut("station/{orgid}")]
		public async Task<IActionResult> UpdateStation([FromRoute] string orgid, [FromBody] StationDto model)
		{

			return Ok(await _orgService.UpdateStationAsync(model, orgid));
		}
		[HttpGet("stations/{orgId}")]
		public async Task<IActionResult> GetStations([FromRoute] string orgId)
		{
			return Ok(await _orgService.GetStationsAsync(orgId));
		}
		[HttpGet("station/{id}")]
		public async Task<IActionResult> GetStation([FromRoute] int id)
		{
			return Ok(await _orgService.GetStationAsync(id));
		}
		[HttpDelete("station/{id}")]
		public async Task<IActionResult> DeleteStation([FromRoute] int id)
		{
			return Ok(await _orgService.DeleteStationAsync(id));
		}
		[HttpPost("trip/add/{orgId}")]
		public async Task<IActionResult> AddTrip([FromRoute] string orgId, [FromBody] AddTripDto model)
		{
			return Ok(await _orgService.AddTripAsync(model, orgId));
		}

		[HttpPut("trip/{id}")]
		public async Task<IActionResult> UpdateTrip([FromRoute] int id, [FromBody] UpdateTripDto model)
		{
			return Ok(await _orgService.UpdateTripAsync(model, id));
		}
		[HttpGet("trips/{orgId}")]
		public async Task<IActionResult> GetTrips([FromRoute] string orgId)
		{
			return Ok(await _orgService.GetTripsAsync(orgId));
		}
		[HttpGet("trip/{id}")]
		public async Task<IActionResult> GetTrip([FromRoute] int id)
		{
			return Ok(await _orgService.GetTripAsync(id));
		}
		[HttpGet("trip/driver/{orgId}/{id}")]
		//[OrgPermissionAuthorize("OrgPermissions.TestPermissions.Create.1")]
		public async Task<IActionResult> GetTripForDriver([FromRoute] string orgId, [FromRoute] string id)
		{
			return Ok(await _orgService.GetTripsForDriverAsync(orgId, id));
		}
		[HttpGet("trip/user/{orgId}/{name}")]
		public async Task<IActionResult> GetTripForUser([FromRoute] string orgId, [FromQuery] string name)
		{
			return Ok(await _orgService.GetTripsForUserAsync(orgId, name));
		}
		[HttpGet("trip/user/{orgId}/{from}/{to}")]
		public async Task<IActionResult> GetTripForUserWithFromTo([FromRoute] string orgId, [FromQuery] string from, [FromQuery] string to)
		{
			return Ok(await _orgService.GetTripsForUserWithToAndFromAsync(orgId, from, to));
		}
		[HttpDelete("trip/{id}")]
		public async Task<IActionResult> DeleteTrip([FromRoute] int id)
		{
			return Ok(await _orgService.DeleteTripAsync(id));
		}
	}
}
