using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Services.EntitiesServices.OrganizationDriverServices;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationDriverController : ControllerBase
    {
        private readonly IOrganizationDriverService _orgDriver;

        public OrganizationDriverController(IOrganizationDriverService orgDriver)
        {
            _orgDriver = orgDriver;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _orgDriver.GetProfileAsync());
        }
        [HttpDelete("decreaseTripSeat")]
        public async Task<IActionResult> DecreaseSeatByOne(DecreaseOrgTripByOneCommnad command)
        {
            return Ok(await _orgDriver.DecreaseSeatByOneAsync(command));
        }
        [HttpPut("updateArriveTime")]
        public async Task<IActionResult> UpdateArriveTime(UpdateTripArriveTimeCommand command)
        {
            return Ok(await _orgDriver.UpdateArriveTimeAsync(command));
        }
        [HttpPut("updateTripStatus")]
        public async Task<IActionResult> UpdateTripStatus(UpdateOrgTripStatusCommand command)
        {
            return Ok(await _orgDriver.UpdateTripStatusAsync(command));
        }
        [HttpGet("reservations")]
        public async Task<IActionResult> GeAllReservation([FromBody]int tripTimeTable)
        {
            return Ok(await _orgDriver.GeAllReservationAsync(tripTimeTable));
        }
        [HttpGet("getLocation")]
        public async Task<IActionResult> GetTripTimeTableLocation([FromBody]int tripTimeTableId)
        {
            return Ok(await _orgDriver.GetTripTimeTableLocationAsync(tripTimeTableId));
        }
        [HttpGet("currentTrip")]
        public async Task<IActionResult> CurrentTrip()
        {
            var userId = User.FindFirst("uid").Value;

            if (userId is null)
            {
                return Unauthorized();
            }

            return Ok(await _orgDriver.GetCurrentTrip(userId));
        }
        [HttpPut("trip/updateLocation")]
        public async Task<IActionResult> TripLocation(TripLocationUpdateDto tripLocationUpdate)
        {
            var userId = User.FindFirst("uid").Value;

            return Ok(await _orgDriver.UpdateCurrentOrgTripLocationAsync(userId, tripLocationUpdate));
        }

        [HttpPut("trip/takeBreak")]
        public async Task<IActionResult> TaxkeBreak(int tripId)
        {
            return Ok(await _orgDriver.TakeBreakAsync(tripId));
        }
    }
}
