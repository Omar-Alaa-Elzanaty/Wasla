using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Services.EntitiesServices.PassangerServices
{
    public interface IPassangerService
    {
        Task<BaseResponse> GetProfile(string customerId);
        Task<BaseResponse> SearchByUserName(string userName);

        Task<BaseResponse> ReservationAsync(ReservationDto order,string customerId);
        Task<BaseResponse> GetInComingReservations(string customerId);
        Task<BaseResponse> GetFirstInComingReservations(string customerId);
        Task<BaseResponse> GetEndedReservations(string customerId);
        Task<BaseResponse> PassengerCancelReversionAsyn(int reverseId);
        Task<BaseResponse> SeatsRecordsAsync(int tripId);
        Task<BaseResponse> OrganizationRateAsync(OrganizationRateDto model,string customerId);
        Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId,string customerId);
        Task<BaseResponse> GetLinesAsync(string orgId);
        Task<BaseResponse> AddPackagesAsync(PackagesRequestDto model);
        Task<BaseResponse> UpdatePackagesAsync(PackagesRequestDto model, int packageId);
        Task<BaseResponse> GetUserOrgPackagesAsync(string customerId);
        Task<BaseResponse> GetUserPublicPackagesAsync(string customerId);
        Task<BaseResponse> RemovePackageAsync(int packageId);
        Task<BaseResponse> GetFollowers(string userId);
        Task<BaseResponse>GetFollowing(string userId);
        Task<BaseResponse> CreateFollowRequestAsync(string senderId,FollowDto followDto);
        Task<BaseResponse> ConfirmFollowRequestAsync(string userId,string senderId);
        Task<BaseResponse> DeleteFollowRequestAsync(string userId, string senderId);
        Task<BaseResponse> DeleteFollowerAsync(string senderId,FollowDto followDto);
        Task<BaseResponse> GetTripSuggestion(string customerId);

        Task<BaseResponse>AddAdsAsync(PassangerAddAdsDto ads, string customerId);
        Task<BaseResponse> LinesVehiclesCountAsync(string orgId);
        Task<BaseResponse> SearchUser(string request);
        Task<BaseResponse> GetUserBySearch(string userId);
        //Task<BaseResponse> DeleteFollowersAsync(DeleteFromFollowersCommand command);
        Task<BaseResponse> AcceptFollowRequestAsync(AcceptFollowRequestCommand command);
        Task<BaseResponse> DisplayFollowingRequestsAsync(string customerId);

        Task<BaseResponse> GetTripsForUserAsync(string orgId, string lineName);
        Task<BaseResponse> GetTripsForUserWithToAndFromAsync(string orgId, string from, string to);

        Task<BaseResponse> FollowersLocation(string userId);
        Task<BaseResponse> PackagesLocations(string userId);
        Task<BaseResponse> SearchTripsForUserAsync(string from, string to, DateTime? date);
        Task<BaseResponse> EditProfile(string userId, EditCustomerProfileDto model);
        Task<BaseResponse> RequestPublicTrip(PassengerPublicTripRequestDto model, string userId);
    }
}
