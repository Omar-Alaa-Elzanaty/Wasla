﻿using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;
using Wasla.Services.EntitiesServices.PublicDriverServices;
using Wasla.Services.EntitiesServices.VehicleSerivces;

namespace Wasla.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicDriverController : ControllerBase
    {
        private readonly IDriverServices _driverService;
        private readonly IVehicleSrivces _vehicleSrivces;
        public PublicDriverController(
            IDriverServices driverService,
            IVehicleSrivces vehicleSrivces)
        {
            _driverService = driverService;
            _vehicleSrivces = vehicleSrivces;
        }
        [HttpGet("packages/{driverId}")]
        public async Task<IActionResult> GetDriverPackagesAsync([FromRoute] string driverId)
        {
            return Ok(await _driverService.GetDriverPublicPackagesAsync(driverId));
        }
        [HttpGet("packages/request/{driverId}")]
        public async Task<IActionResult> GetPublicPackagesRequestAsync([FromRoute] string driverId)
        {
            return Ok(await _driverService.GetPublicPackagesRequestAsync(driverId));
        }
        [HttpPut("package/{packageId}/{status}")]
        public async Task<IActionResult> ReviewPackagesRequest(int packageId,PackageStatus status)
        {
            return Ok(await _driverService.ReviewPackagesRequest(packageId,status));
        }
        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            return Ok(await _driverService.GetProfileAsync(userId));
        }
        [HttpGet("currentTripStatus")]
        public async Task<IActionResult> GetcurrentTripStatus(string userId)
        {
            return Ok(await _driverService.GetTripStatus(userId));
        }
        [HttpPut("updateTripStatus")]
        public async Task<IActionResult> UpdateTripStatus(int tripId,TripStatus status)
        {
            return Ok(await _driverService.UpdateTripStatus(tripId, status));
        }
        [HttpPost("createPublicTrip")]
        public async Task<IActionResult>CreatePublicTrip(CreatePublicDriverCommand command,string userId)
        {
            return Ok(await _driverService.CreatePublicTrip(userId, command));
        }
        [HttpPut("updateProfile")]
        public async Task<IActionResult> UpdateProflie(UpdatePublicDriverProfileCommand command)
        {
            return Ok(await _driverService.UpdatePublicTrip(command));
        }
        [HttpGet("tripLine")]
        public async Task<IActionResult>TripLine(int tripId)
        {
            return Ok(await _driverService.GetTripLine(tripId));
        }
        [HttpPut("update/trip/start/{tripId}")]
        public async Task<IActionResult> UpdatePublicTripStart(int tripId)
        {
            return Ok(await _driverService.UpdatePublicTripStart(tripId));
        }
        [HttpGet("trip/byDate/{date}")]
        public async Task<IActionResult> GetPublicTripsByDate(DateTime date)
        {
            return Ok(await _driverService.GetPublicTripsByDate(date));
        }
        [HttpPut("trip/reservation/ByOne/{tripId}")]
        public async Task<IActionResult> UpdateTripReservationByOne(int tripId)
        {
            return Ok(await _driverService.UpdateTripReservationByOne(tripId));
        }
        [HttpGet("trip/status/{tripId}")]
        public async Task<IActionResult> GetPublicTripState(int tripId)
        {
            return Ok(await _driverService.GetPublicTripState(tripId));
        }
        [HttpGet("trip/reservation/onRoad/{tripId}")]
        public async Task<IActionResult> GetReservationOnRoad(int tripId )
        {
            return Ok(await _driverService.GetReservationOnRoad(tripId));
        }
        [HttpPut("trip/updateLocation")]
        public async Task<IActionResult> TripLocation(TripLocationUpdateDto tripLocationUpdate)
        {
            var userId = User.FindFirst("uid").Value;

            return Ok(await _driverService.UpdateCurrentPublicTripLocationAsync(userId, tripLocationUpdate));
        }
        [HttpPut("trips/status")]
        public async Task<IActionResult> UpdatePublicTripStatus()
        {
            var driverId = User.FindFirst("uid").Value;

            return Ok(await _driverService.UpdatePublicTripsStatus(driverId));
        }
        [HttpGet("currentTrip")]
        public async Task<IActionResult> CurrentTrip()
        {
            var userId = User.FindFirst("uid").Value;

            if(userId is null)
            {
                return Unauthorized();
            }

            return Ok(await _driverService.GetCurrentTrip(userId));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _vehicleSrivces.GetVehicleById(id));
        }
    }
}
