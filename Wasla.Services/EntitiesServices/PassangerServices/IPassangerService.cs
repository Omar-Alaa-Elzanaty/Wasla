using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Services.EntitiesServices.PassangerServices
{
    public interface IPassangerService
    {
        Task<BaseResponse> GetProfile();
        Task<BaseResponse> ReservationAsync(ReservationDto order);
        Task<BaseResponse> GetInComingReservations();
        Task<BaseResponse> GetEndedReservations();
        Task<BaseResponse> PassengerCancelReversionAsyn(int reverseId);
        Task<BaseResponse> SeatsRecordsAsync(int tripId);
        Task<BaseResponse> OrganizationRateAsync(OrganizationRateDto model);
        Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId);
        Task<BaseResponse> GetLinesAsync(string orgId);
        Task<BaseResponse> AddPackagesAsync(PackagesRequestDto model);
        Task<BaseResponse> UpdatePackagesAsync(PackagesRequestDto model, int packageId);
        Task<BaseResponse> GetUserOrgPackagesAsync();
        Task<BaseResponse> GetUserPublicPackagesAsync();
        Task<BaseResponse> RemovePackageAsync(int packageId);
        Task<BaseResponse> CreateFollowRequestAsync(FollowDto followDto);
        Task<BaseResponse> ConfirmFollowRequestAsync(FollowDto followDto);
        Task<BaseResponse> DeleteFollowRequestAsync(FollowDto followDto);
        Task<BaseResponse> DeleteFollowerAsync(FollowDto followDto);
        Task<BaseResponse> GetTripSuggestion();

        Task<BaseResponse>AddAdsAsync(PassangerAddAdsDto ads);
        Task<BaseResponse> LinesVehiclesCountAsync(string orgId);
    }
}
