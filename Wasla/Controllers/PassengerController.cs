using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls;
using Wasla.Model.Dtos;
using Wasla.Model.Models;
using Wasla.Services.EntitiesServices.PassangerServices;
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
		[HttpPost("/trip/reserve")]
		public async Task<IActionResult> ReserveTicket([FromBody] ReservationDto order)
		{
			return Ok(await _passangerService.ReservationAsync(order));
		}
		[HttpPost("organization/rate")]
		public async Task<IActionResult> RateOrganize([FromBody] OrganizationRateDto model)
		{
			return Ok(await _passangerService.OrganizationRateAsync(model));
		}
		[HttpDelete("organization/rate/remove")]
		public async Task<IActionResult> RemoveOrganizationRate(string orgainzationId)
		{
			return Ok(await _passangerService.OrganizationRateRemoveAsync(orgainzationId));
		}
		[HttpGet("lines/{orgId}")]
		public async Task<IActionResult> GetLines([FromRoute] string orgId)
		{
			return Ok(await _passangerService.GetLinesAsync(orgId));
		}
		[HttpPost("Package/add")]
		public async Task<IActionResult> AddPackage([FromForm] PackagesRequestDto model)
		{
			return Ok(await _passangerService.AddPackagesAsync(model));
		}
		[HttpPut("package/{id}")]
		public async Task<IActionResult> UpdatePackage([FromRoute] int id, [FromForm] PackagesRequestDto model)
		{
			return Ok(await _passangerService.UpdatePackagesAsync(model, id));
		}
		[HttpDelete("package/{id}")]
		public async Task<IActionResult> DeletePackage([FromRoute] int id)
		{
			return Ok(await _passangerService.RemovePackageAsync(id));
		}
		[HttpGet("packages")]
		public async Task<IActionResult> GetUserPublicPackagesAsync()
		{
			return Ok(await _passangerService.GetUserPublicPackagesAsync());
		}
		[HttpGet("packages/organization/{userName}")]
		public async Task<IActionResult> GetUserOrgPackagesAsync()
		{
			return Ok(await _passangerService.GetUserOrgPackagesAsync());
		}

		[HttpPost("advertisment/{customerId}")]
		public async Task<IActionResult> AddAdvertisment([FromForm] PassangerAddAdsDto request)
		{
			return Ok(await _passangerService.AddAdsAsync(request));
		}
		[HttpGet("linesVehicles/{orgId}")]
		public async Task<IActionResult> LinesVehiclesCount(string orgId)
		{
			return Ok(await _passangerService.LinesVehiclesCountAsync(orgId));
		}
		[HttpDelete("cancelReverse/{reverseId}")]
		public async Task<IActionResult> PassengerCancelReversionAsyn(int reverseId)
		{
            return Ok(await _passangerService.PassengerCancelReversionAsyn(reverseId));
        }
		[HttpGet("profile")]
		public async Task<IActionResult> DisplayProfile()
		{
			return Ok(await _passangerService.GetProfile());
		}
		[HttpGet("incomingTrips")]
		public async Task<IActionResult>GetIncomingTrips()
		{
			return Ok(await _passangerService.GetInComingReservations());
		}
		[HttpGet("endedTrips")]
		public async Task<IActionResult>GetEndedTrips()
		{
			return Ok(await _passangerService.GetEndedReservations());
		}
		[HttpGet("tripsSuggestions")]
		public async Task<IActionResult>First3TripsSuggestion()
		{
			return Ok(await _passangerService.GetTripSuggestion());
		}
    }
	
}
