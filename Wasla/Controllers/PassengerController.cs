using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
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

		[HttpGet("tripSeats/{tripId}")]
		public async Task<IActionResult> SeatsStatus(int tripId)
		{
			return Ok(await _passangerService.SeatsRecordsAsync(tripId));
		}
		[HttpPost("{userId}/trip/reserve/{tripId}")]
		public async Task<IActionResult> ReserveTicket([FromBody] ReservationDto order)
		{
			return Ok(await _passangerService.ReservationAsync(order));
		}
		[HttpPost("organization/rate")]
		public async Task<IActionResult> RateOrganize([FromBody] OrganizationRate model)
		{
			return Ok(await _passangerService.OrganizationRateAsync(model));
		}
		[HttpDelete("organization/rate/remove")]
		public async Task<IActionResult> RemoveOrganizationRate(string orgainzationId, string customerId)
		{
			return Ok(await _passangerService.OrganizationRateRemoveAsync(orgainzationId, customerId));
		}
	}
}
