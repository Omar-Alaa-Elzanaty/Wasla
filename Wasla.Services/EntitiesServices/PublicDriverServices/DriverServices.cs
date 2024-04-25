﻿using System.Globalization;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;
using Wasla.Services.EntitiesServices.OrganizationSerivces;
using Wasla.Services.Exceptions;

namespace Wasla.Services.EntitiesServices.PublicDriverServices
{
    public class DriverServices : IDriverServices
    {
        private readonly WaslaDb _context;
        private readonly BaseResponse _response;
        private readonly IStringLocalizer<OrganizationSerivce> _localization;
        private readonly IMapper _mapper;
        public DriverServices(
            WaslaDb context,
            IStringLocalizer<OrganizationSerivce> localization,
            IMapper mapper)
        {
            _context = context;
            _response = new();
            _localization = localization;
            _mapper = mapper;
        }
        public async Task<BaseResponse> GetPublicPackagesRequestAsync(string driverId)
        {
            var packages = await _context.Packages.Where(p => p.DriverId != null && p.DriverId == driverId && p.Status == 0).ToListAsync();
            var res = _mapper.Map<List<DriverPackagesDto>>(packages);
            _response.Data = res;
            return _response;
        }

        public async Task<BaseResponse> ReviewPackagesRequest(int packageId, int status)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);

            if (package is null)
            {
                throw new KeynotFoundException(_localization["ObjectNotFound"].Value);
            }
            package.Status = status;
            await _context.SaveChangesAsync();
            _response.Message = _localization["PackageReviewedSuccess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetDriverPublicPackagesAsync(string DriverId)
        {
            var packages = await _context.Packages.Where(p => p.DriverId == DriverId).ToListAsync();
            var res = _mapper.Map<List<DriverPackagesDto>>(packages);
            _response.Data = res;
            return _response;
        }
        public async Task<BaseResponse> GetProfileAsync(string userId)
        {
            var entity = await _context.PublicDrivers.FindAsync(userId);

            if (entity is null)
            {
                _response.IsSuccess = false;
                _response.Message = _localization["UserNameNotFound"].Value;
                return _response;
            }

            var publicDriver = _mapper.Map<PubliDriverProfileDto>(entity);

            _response.Data = publicDriver;
            return _response;
        }
        public async Task<BaseResponse> GetTripStatus(string userId)
        {
            var trip = await _context.PublicDriverTrips.SingleOrDefaultAsync(x => x.PublicDriverId == userId && x.IsActive == true);

            if (trip is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["NoActiveTrip"].Value);
            }

            _response.Data = new PublicTripStatusQueryDto()
            {
                Status = trip.Status,
                ReserverdSeats = trip.ReservedSeats,
                TotalTripSeats = _context.Vehicles.First(x => x.PublicDriverId == userId).Capcity
            };

            return _response;
        }
        public async Task<BaseResponse> UpdateTripStatus(int tripId, TripStatus status)
        {
            var trip = await _context.PublicDriverTrips.FindAsync(tripId);

            if (trip is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["NoActiveTrip"].Value);
            }

            trip.Status = status;

            return _response;
        }
        public async Task<BaseResponse> CreatePublicTrip(string userId, CreatePublicDriverCommand command)
        {
            var publicDriverTrip = _mapper.Map<PublicDriverTrip>(command);
            publicDriverTrip.PublicDriverId = userId;
            publicDriverTrip.IsActive = true;
            publicDriverTrip.AcceptPackages = publicDriverTrip.AcceptRequests = true;
            publicDriverTrip.Status = TripStatus.None;

            await _context.AddAsync(publicDriverTrip);
            await _context.SaveChangesAsync();

            _response.Data = publicDriverTrip.Id;
            return _response;
        }
        public async Task<BaseResponse> UpdatePublicTrip(UpdatePublicDriverProfileCommand command)
        {
            var entity = await _context.PublicDrivers.FindAsync(command.Id);

            if (entity is null)
            {
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["UserNotFound"].Value);
            }

            _mapper.Map(command, entity);

            _context.PublicDrivers.Update(entity);
            await _context.SaveChangesAsync();

            _response.Message = _localization["SuccessProcess"].Value;
            return _response;
        }
        public async Task<BaseResponse> GetTripLine(int tripId)
        {
            var trip = await _context.PublicDriverTrips.FindAsync(tripId);

            if (trip is null)
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.NotFound, _localization["NoActiveTrip"].Value);

            var start = trip.StartStation;
            var end = trip.EndStation;

            _response.Data = new PublicTripLineQueryDto()
            {
                StartStation = start.Name,
                StartLangtitude = start.Langtitude,
                StartLatitude = start.Latitude,
                EndStation = end.Name,
                EndLangtitude = end.Langtitude,
                EndLatitude = end.Latitude,
            };

            return _response;
        }

        public async Task<BaseResponse> UpdatePublicTripStart(int tripId)
        {
            var trip = await _context.PublicDriverTrips.FindAsync(tripId);
            if (trip is null)
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["NoActiveTrip"].Value);
            trip.IsActive = false;
            trip.IsStart = true;
            trip.AcceptPackages = false;
            trip.Status = TripStatus.OnRoad;
            _response.Message = _localization["SuccessProcess"].Value;
            return _response;
        }

        public async Task<BaseResponse> GetPublicTripsByDate(string date)
        {

            if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var searchDate))
            {
                var trips = await _context.PublicDriverTrips.Where(t => t.StartDate.Date == searchDate).ToListAsync();
                var tripRes = _mapper.Map<List<PublicTripDto>>(trips);
                _response.Data = tripRes;
            }
            else
            {
                _response.Message = _localization["NotPublicTripInDate"].Value;
            }
            return _response;
        }
        public async Task<BaseResponse> UpdateTripReservationByOne(int tripId)
        {
            var trip = await _context.PublicDriverTrips.FindAsync(tripId);
            if (trip is null)
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["TripNotFound"].Value);
            trip.ReservedSeats += 1;
            _context.PublicDriverTrips.Update(trip);
            await _context.SaveChangesAsync();
            _response.Message = _localization["SuccessProcess"].Value;
            return _response;

        }
        public async Task<BaseResponse> GetPublicTripState(int tripId)
        {
            var trip = await _context.PublicDriverTrips.FindAsync(tripId);
            if (trip is null)
                return BaseResponse.GetErrorException(System.Net.HttpStatusCode.BadRequest, _localization["TripNotFound"].Value);
            _response.Data = trip.Status;
            return _response;
        }
        public async Task<BaseResponse> GetReservationOnRoad(int tripId)
        {
            var reservations = await _context.PublicDriverTripRequests.Where(t => t.PublicDriverTripId == tripId && t.OnRoad == true).Select(t => new PublicTriptReservationRequestDto
            {
                TripTime = t.PublicDriverTrip.StartDate.ToString(@"hh\:mm\:ss"),
                customerName = t.Customer.FirstName,
                StartStation = t.PublicDriverTrip.StartStation.Name,
                EndStation = t.PublicDriverTrip.EndStation.Name,
                CustomerReservationId = t.Id
            }).ToListAsync();
            _response.Data = reservations;
            return _response;
        }

       
    }
}
