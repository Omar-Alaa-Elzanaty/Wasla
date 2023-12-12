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
		Task<BaseResponse> DisplayVehicles(string? orgId);
		Task<BaseResponse> AddVehicleAsync(VehicleDto vehicleModel, string orgId);
		Task<BaseResponse> VehicleAnalysisAsync(string orgId);
		Task<BaseResponse> UpdateVehicleAsync(VehicleDto model, int vehicleId);
		Task<BaseResponse> DeleteVehicleAsync(int vehicleId);
		Task<BaseResponse> AddDriverAsync(OrgDriverDto model, string? orgId);
		Task<BaseResponse> AddEmployeeAsync(EmployeeRegisterDto model, string? orgId);
		Task<BaseResponse> DeleteEmployeeAsync(string employeeId);
		Task<BaseResponse> GetAllDrivers(string? orgId);
		#region Ads
		Task<BaseResponse> AddAdsAsync(AdsDto model);
		Task<BaseResponse> AddAdsToVehicleAsync(int adsId, int vehicleId);
		Task<BaseResponse> ReomveAdsFromVehicleAsync(int adsId, int vehicleId);
		Task<BaseResponse> UpdateAdsAsync(int adsId, AdsDto model);
		Task<BaseResponse> DeleteAdsAsync(int adsId); 
		#endregion
	}
}
