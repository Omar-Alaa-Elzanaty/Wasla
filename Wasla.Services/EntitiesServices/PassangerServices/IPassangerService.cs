using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Services.EntitiesServices.PassangerServices
{
    public interface IPassangerService
    {
        Task<BaseResponse> GetProfile(string userId);
        Task<BaseResponse> ReservationAsync(ReservationDto order);
        Task<BaseResponse> GetInComingReservations(string userId);
        Task<BaseResponse> GetEndedReservations(string userId);
        Task<BaseResponse> PassengerCancelReversionAsyn(int reverseId);
        Task<BaseResponse> SeatsRecordsAsync(int tripId);
        Task<BaseResponse> OrganizationRateAsync(OrganizationRate model);
        Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId, string customerId);
        Task<BaseResponse> GetLinesAsync(string orgId);
        Task<BaseResponse> AddPackagesAsync(PackagesRequestDto model);
        Task<BaseResponse> UpdatePackagesAsync(PackagesRequestDto model, int packageId);
        Task<BaseResponse> GetUserOrgPackagesAsync(string userName);
        Task<BaseResponse> GetUserPublicPackagesAsync(string userName);
        Task<BaseResponse> RemovePackageAsync(int packageId);
        Task<BaseResponse> GetTripSuggestion(string userId);

        Task<BaseResponse>AddAdsAsync(string customerId,PassangerAddAdsDto ads);
        Task<BaseResponse> LinesVehiclesCountAsync(string orgId);
    }
}
