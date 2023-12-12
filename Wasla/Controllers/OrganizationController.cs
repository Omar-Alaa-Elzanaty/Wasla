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
		[HttpGet("vehicle/displayAll")]
		public async Task<IActionResult> DisplayVehicles()
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			string? orgId = null;
			if (userName is not null)
			{
				orgId = _userManager.FindByNameAsync(userName).Result?.Id;
			}

			return Ok(await _orgService.DisplayVehicles(orgId));
		}
		[HttpPost("vehicle/add")]
		public async Task<IActionResult> AddVehicle([FromForm] VehicleDto model)
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			string? orgId = null;
			if (userName is not null)
			{
				orgId = _userManager.FindByNameAsync(userName).Result?.Id;
			}

			return Ok(await _orgService.AddVehicleAsync(model, orgId));
		}
		[HttpPut("vehicle/update/{vehicleId}")]
		public async Task<IActionResult> UpdateVehicle(int vehicleId,[FromForm]VehicleDto model)
		{

			return Ok(await _orgService.UpdateVehicleAsync(model, vehicleId));
		}
		[HttpDelete("vehicle/delete")]
		public async Task<IActionResult>DeleteVehicle(int vehicleId)
		{
			return Ok(await _orgService.DeleteVehicleAsync(vehicleId));
		}
		[HttpPost("driver/add")]
		public async Task<IActionResult> AddDriver([FromForm]OrgDriverDto model)
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			string? orgId = null;
			if (userName is not null)
			{
				orgId = _userManager.FindByNameAsync(userName).Result?.Id;
			}

			return Ok(await _orgService.AddDriverAsync(model,orgId));
		}
		[HttpGet("vehicle/vehicleAnalysis")]
		public async Task<IActionResult> VehicleAnalysis()
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			string? orgId = null;
			if (userName is not null)
			{
				orgId = _userManager.FindByNameAsync(userName).Result?.Id;
			}

			return Ok(await _orgService.VehicleAnalysisAsync(orgId??""));
		}
		[HttpPost("employee/add")]
		public async Task<IActionResult> AddEmployee([FromForm]EmployeeRegisterDto model)
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			string? orgId = null;
			if(userName is not null)
			{
				orgId = _userManager.FindByNameAsync(userName).Result?.Id;
			}

			return Ok(await _orgService.AddEmployeeAsync(model,orgId));
		}
		[HttpDelete("employee/delete/{empId}")]
		public async Task<IActionResult>DeleteEmployee(string empId)
		{
			return Ok(await _orgService.DeleteEmployeeAsync(empId));
		}
		[HttpGet("drvier/displayAll")]
		public async Task<IActionResult> GetAllDrivers()
		{
			var userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			string? orgId = null;
			if (userName is not null)
			{
				orgId = _userManager.FindByNameAsync(userName).Result?.Id;
			}

			return Ok(await _orgService.GetAllDrivers(orgId));
		}
	}
}
