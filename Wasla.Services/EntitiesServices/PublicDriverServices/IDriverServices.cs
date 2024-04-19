using System;
using System.Collections.Generic;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Enums;

namespace Wasla.Services.EntitiesServices.PublicDriverServices
{
    public interface IDriverServices
    {
        Task<BaseResponse> GetPublicPackagesRequestAsync(string driverId);
        Task<BaseResponse> ReviewPackagesRequest(int packageId, int status);
        Task<BaseResponse> GetDriverPublicPackagesAsync(string DriverId);
        Task<BaseResponse> GetProfileAsync(string userId);
        Task<BaseResponse> GetTripStatus(string userId);
        Task<BaseResponse> UpdateTripStatus(int tripId, TripStatus status);
        Task<BaseResponse> CreatePublicTrip(string userId, CreatePublicDriverCommand command);
        Task<BaseResponse> UpdatePublicTrip(UpdatePublicDriverProfileCommand command);
        Task<BaseResponse> GetTripLine(int tripId);
        Task<BaseResponse> UpdatePublicTripStart(int tripId);
        Task<BaseResponse> GetPublicTripsByDate(string date);
        Task<BaseResponse> UpdateTripReservationByOne(int tripId);
        Task<BaseResponse> GetPublicTripState(int tripId);
        Task<BaseResponse> GetReservationOnRoad(int tripId);
    }
}
