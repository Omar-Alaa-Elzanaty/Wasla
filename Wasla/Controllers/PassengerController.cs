using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Wasla.Services.PassangerServices;
namespace Wasla.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPassangerService _passangerService;
		public PassengerController(IMapper mapper, IPassangerService passangerService)
		{
			_mapper = mapper;
			_passangerService = passangerService;
		}

		[HttpGet("tripSets/{tripId}")]
		public async Task<IActionResult> SetsStatus(int tripId)
		{
			return Ok(await _passangerService.SetsRecordsAsync(tripId));
		}
		[HttpPost("trip/Reserve/{tripId}")]
		public async Task<IActionResult> ReserveTicket([FromBody] List<int> SetsNumbers,int tripId)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			return Ok(await _passangerService.ReservationAsync(SetsNumbers, tripId, userId));
		}
	}
}
