using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Wasla.Model.Dtos;
using Wasla.Model.Models;
using Wasla.Services.OrganizationSerivces;

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
	}
}
