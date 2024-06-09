﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Services.EntitiesServices.PassangerServices;
namespace Wasla.Api.Controllers
{
    [Route("api/passanger")]
    [ApiController]
    [Authorize("")]
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
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.ReservationAsync(order, userId));
        }
        [HttpPost("organization/rate")]
        public async Task<IActionResult> RateOrganize([FromBody] OrganizationRateDto model)
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.OrganizationRateAsync(model, userId));
        }
        [HttpDelete("organization/rate/remove")]    
        public async Task<IActionResult> RemoveOrganizationRate(string orgainzationId)
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.OrganizationRateRemoveAsync(orgainzationId, userId));
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
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.GetUserPublicPackagesAsync(userId));
        }
        [HttpGet("packages/organization/{userName}")]
        public async Task<IActionResult> GetUserOrgPackagesAsync()
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.GetUserOrgPackagesAsync(userId));
        }

        [HttpPost("advertisment/{customerId}")]
        public async Task<IActionResult> AddAdvertisment([FromForm] PassangerAddAdsDto request)
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.AddAdsAsync(request, userId));
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
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.GetProfile(userId));
        }
        [HttpGet("incomingTrips")]
        public async Task<IActionResult> GetIncomingTrips()
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.GetInComingReservations(userId));
        }
        [HttpGet("firstincomingTrip")]
        public async Task<IActionResult> GetFirstIncomingTrip()
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.GetFirstInComingReservations(userId));
        }
        [HttpGet("endedTrips")]
        public async Task<IActionResult> GetEndedTrips()
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.GetEndedReservations(userId));
        }
        [HttpGet("tripsSuggestions")]
        public async Task<IActionResult> FirstTripsSuggestion()
        {
            var userId = User.FindFirst("uid").Value;

            return Ok(await _passangerService.GetTripSuggestion(userId));
        }
        [HttpPost("createFollowRequest")]
        public async Task<IActionResult> CreateFollowRequest(FollowDto followDto)
        {
            var senderId = User.FindFirst("uid").Value;

            return Ok(await _passangerService.CreateFollowRequestAsync(senderId,followDto));
        }
        [HttpPost("ConfirmFollowRequest")]
        public async Task<IActionResult> ConfirmFollowRequest(string senderId)
        {
            var userId = User.FindFirst("uid").Value;

            return Ok(await _passangerService.ConfirmFollowRequestAsync(userId,senderId));
        }
        [HttpDelete("rejectFollowRequest")]
        public async Task<IActionResult> DeleteFollowRequest([FromBody] DeleteFollowRequestDto followRequest)
        {
            var userId = User.FindFirst("uid").Value;

            return Ok(await _passangerService.DeleteFollowRequestAsync(userId,followRequest.SenderId));
        }
        [HttpDelete("deleteFollower")]
        public async Task<IActionResult> DeleteFollower(FollowDto followDto)
        {
            var senderId = User.FindFirst("uid").Value;

            return Ok(await _passangerService.DeleteFollowerAsync(senderId,followDto));
        }
        //[HttpPost("acceptFollowRequest")]
        //public async Task<IActionResult> AcceptRequest([FromBody]string follwerId)
        //{
        //    var userId = User.FindFirst("uid").Value;
        //    return Ok(await _passangerService.AcceptFollowRequestAsync(new AcceptFollowRequestCommand()
        //    {
        //        FolowerId = follwerId,
        //        SenderId = userId
        //    }));
        //}
        [HttpGet("displayFollowRequest")]
        public async Task<IActionResult> DisplayFollowRequest()
        {
            var userId = User.FindFirst("uid").Value;
            return Ok(await _passangerService.DisplayFollowingRequestsAsync(userId));
        }
        [HttpGet("getFollowing")]
        public async Task<IActionResult> GetFollowing()
        {
            var userId = User.FindFirst("uid").Value;

            if (userId is null)
            {
                return BadRequest("user not found");
            }

            return Ok(await _passangerService.GetFollowing(userId));
        }
        [HttpGet("getFollower")]
        public async Task<IActionResult> GetFollower()
        {
            var userId = User.FindFirst("uid").Value;

            if (userId is null)
            {
                return BadRequest("user not found");
            }

            return Ok(await _passangerService.GetFollowers(userId));
        }

        [HttpGet("trip/user/{orgId}/{lineName}")]
        public async Task<IActionResult> GetTripForUser([FromRoute] string orgId, [FromRoute] string lineName)
        {
            return Ok(await _passangerService.GetTripsForUserAsync(orgId, lineName));
        }
        [HttpGet("search/trips/user/{from}/{to}/date")]
        public async Task<IActionResult> SearchTripsForUser([FromRoute] string from, [FromRoute] string to, [FromQuery] DateTime? date)
        {
            return Ok(await _passangerService.SearchTripsForUserAsync(from, to,date));
        }

        [HttpGet("trip/user/{orgId}/{from}/{to}")]
        public async Task<IActionResult> GetTripForUserWithFromTo([FromRoute] string orgId, [FromRoute] string from, [FromRoute] string to)
        {
            return Ok(await _passangerService.GetTripsForUserWithToAndFromAsync(orgId, from, to));
        }

        [HttpGet("followerTrips")]
        public async Task<IActionResult> GetFollowersTrips()
        {
            var userId = User.FindFirst("uid")?.Value;

            if (userId is null)
            {
                return BadRequest("user not found");
            }

            return Ok(await _passangerService.FollowersLocation(userId));
        }

        [HttpGet("packagesTrips")]
        public async Task<IActionResult> GetPackagesTrips()
        {
            var userId = User.FindFirst("uid")?.Value;

            if (userId is null)
            {
                return BadRequest("user not found");
            }

            return Ok(await _passangerService.PackagesLocations(userId));
        }
        [HttpGet("searchByUserName")]
        public async Task<IActionResult> SearchByUserName(string userName)
        {
            return Ok(await _passangerService.SearchByUserName(userName));
        }
        [HttpPut("EditProflie")]
        public async Task<IActionResult>UpdateProfile(EditCustomerProfileDto model)
        {
            var userId = User.FindFirst("uid")?.Value;

            if (userId is null)
            {
                return BadRequest("user not found");
            }

            return Ok(await _passangerService.EditProfile(userId,model));
        }

        [HttpPost("reqeustPublicTrip")]
        public async Task<IActionResult>RequestPublicTrip(PassengerPublicTripRequestDto model)
        {
            var userId = User.FindFirst("uid")?.Value;

            if (userId is null)
            {
                return BadRequest("user not found");
            }

            return Ok(await _passangerService.RequestPublicTrip(model, userId));
        }
    }

}
