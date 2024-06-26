﻿
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.EntitiesServices.OrganizationSerivces
{
    public interface IOrganizationService
    {
        Task<BaseResponse> DisplayVehicles(string orgId);
        Task<BaseResponse> AddVehicleAsync(VehicleDto vehicleModel, string orgId);
        Task<BaseResponse> VehicleAnalysisAsync(string orgId);
        Task<BaseResponse> UpdateVehicleAsync(VehicleDto model, int vehicleId);
        Task<BaseResponse> DeleteVehicleAsync(int vehicleId);
        Task<BaseResponse> AddDriverAsync(OrgDriverDto model, string orgId);
        Task<BaseResponse> DeleteEmployeeAsync(string employeeId);
        Task<BaseResponse> GetAllDrivers(string orgId);
        #region Ads
        Task<BaseResponse> AddAdsAsync(AdsDto model, string orgId);
        Task<BaseResponse> AddAdsToVehicleAsync(int adsId, int vehicleId);
        Task<BaseResponse> RemoveAdsFromVehicleAsync(int adsId, int vehicleId);
        Task<BaseResponse> UpdateAdsAsync(int adsId, AdsDto model);
        Task<BaseResponse> DeleteAdsAsync(int adsId);
        #endregion
        Task<BaseResponse> AddEmployeeAsync(EmployeeRegisterDto model, string? orgId);
        #region station
        Task<BaseResponse> AddStationAsync(StationDto model, string orgId);
        Task<BaseResponse> UpdateStationAsync(StationDto stationDto, string orgId, int stationId);
        Task<BaseResponse> GetStationsAsync(string orgId);
        Task<BaseResponse> GetStationAsync(int id);
        Task<BaseResponse> DeleteStationAsync(int id);
        #endregion
        #region lines
        Task<BaseResponse> AddLineAsync(LineRequestDto lineDto);
        Task<BaseResponse> UpdateLineAsync(LineRequestDto lineDto, int lineId);
        Task<BaseResponse> GetLinessAsync(string orgId);
        Task<BaseResponse> GetLineAsync(int id);
        Task<BaseResponse> DeleteLineAsync(int id);
        Task<BaseResponse> GetTripsByLineIdAsync(string orgId, int lineId);
        #endregion
        #region trip
        Task<BaseResponse> AddTripAsync(AddTripDto model, string orgId);
        Task<BaseResponse> UpdateTripAsync(UpdateTripDto model, int id);
        Task<BaseResponse> GetTripsAsync(string orgId);
        Task<BaseResponse> GetTripAsync(int id);
        #endregion
        Task<BaseResponse> GetTripsForDriverAsync(string orgId, string driverId);
        Task<BaseResponse> GetTripsForUserAsync(string orgId, string lineName);
        Task<BaseResponse> GetTripsTimeByTripIdAndDate(int tripId,string date);

        Task<BaseResponse> GetTripsForUserWithToAndFromAsync(string orgId, string from, string to);
        Task<BaseResponse> DeleteTripAsync(int id);
        Task<BaseResponse> GetOriganizationsWithName(string name);
        #region Trip Time Table
        Task<BaseResponse> AddTripTimeAsync(AddTripTimeDto model);
        Task<BaseResponse> UpdateTripTimeAsync(UpdateTripTimeDto model, int id);
        Task<BaseResponse> GetTripsTimeAsync(string orgId);
        Task<BaseResponse> GetTripTimeAsync(int id);
        Task<BaseResponse> DeleteTripTimeAsync(int id);

        #endregion
        #region pack
        
        Task<BaseResponse> GetPackageAsync(int packageId);
        Task<BaseResponse> GetPackagesTripAsync(int tripId);
        //Task<BaseResponse> GetUserPackagesAsync(string userName);
        
        Task<BaseResponse> GetPackagesRequestAsync(string orgId);
        Task<BaseResponse> ReviewPackagesRequest(int packageId, int status);
        #endregion
    }
}
