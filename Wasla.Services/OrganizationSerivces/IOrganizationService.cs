using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.OrganizationSerivces
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
		Task<BaseResponse> AddAdsAsync(AdsDto model,string orgId);
		Task<BaseResponse> AddAdsToVehicleAsync(int adsId, int vehicleId);
		Task<BaseResponse> RemoveAdsFromVehicleAsync(int adsId, int vehicleId);
		Task<BaseResponse> UpdateAdsAsync(int adsId, AdsDto model);
		Task<BaseResponse> DeleteAdsAsync(int adsId); 
		#endregion
		Task<BaseResponse> AddEmployeeAsync(EmployeeRegisterDto model, string? orgId);
        Task<BaseResponse> AddStationAsync(StationDto model, string orgId);
        Task<BaseResponse> UpdateStationAsync(StationDto model,string orgId);
        Task<BaseResponse> GetStationsAsync(string orgId);
        Task<BaseResponse> GetStationAsync(int id);
        Task<BaseResponse> DeleteStationAsync(int id);
        Task<BaseResponse> AddTripAsync(AddTripDto model, string orgId);
        Task<BaseResponse> UpdateTripAsync(UpdateTripDto model, int id);
		Task<BaseResponse> GetTripsAsync(string orgId);
        Task<BaseResponse> GetTripAsync(int id);
		Task<BaseResponse> GetTripsForDriverAsync(string orgId, string driverId);
        Task<BaseResponse> GetTripsForUserAsync(string orgId, string name);
        Task<BaseResponse> GetTripsForUserWithToAndFromAsync(string orgId, string from, string to);
        Task<BaseResponse> DeleteTripAsync(int id);
		Task<BaseResponse> GetOriganizationsWithName(string name);
    }
}
