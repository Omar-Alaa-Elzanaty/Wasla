using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wasla.Model.Models;
using Wasla.Services.PassangerServices;
namespace Wasla.Api.Controllers
{
    [Route("api/passanger")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IPassangerService _passangerService;
		public PassengerController(IPassangerService passangerService)
		{
			_passangerService = passangerService;
		}

		[HttpGet("tripSets/{tripId}")]
		public async Task<IActionResult> SetsStatus(int tripId)
		{
			return Ok(await _passangerService.SetsRecordsAsync(tripId));
		}
		[HttpPost("{userId}/trip/reserve/{tripId}")]
		public async Task<IActionResult> ReserveTicket([FromBody] List<int> SetsNumbers,int tripId,string userId)
		{
			return Ok(await _passangerService.ReservationAsync(SetsNumbers, tripId, userId));
		}
		[HttpPost("organization/rate")]
		public async Task<IActionResult> RateOrganize([FromBody] OrganizationRate model)
		{
			return Ok(await _passangerService.OrganizationRateAsync(model));
		}
		[HttpDelete("organization/rate/remove")]
		public async Task<IActionResult>RemoveOrganizationRate(string orgainzationId,string customerId)
		{
			return Ok(await _passangerService.OrganizationRateRemoveAsync(orgainzationId, customerId));
		}
	}
}
