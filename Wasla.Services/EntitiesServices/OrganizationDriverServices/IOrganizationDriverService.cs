using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.EntitiesServices.OrganizationDriverServices
{
    public interface IOrganizationDriverService
    {
        Task<BaseResponse> GetProfileAsync();
        Task<BaseResponse> DecreaseSeatByOneAsync(DecreaseOrgTripByOneCommnad command);
        Task<BaseResponse> UpdateArriveTimeAsync(UpdateTripArriveTimeCommand command);
        Task<BaseResponse> UpdateTripStatusAsync(UpdateOrgTripStatusCommand command);
        Task<BaseResponse> GeAllReservationAsync(int tripTimeTableId);
        Task<BaseResponse> UpdateCurrentOrgTripLocationAsync(string driverId, TripLocationUpdateDto tripDto);
        Task<BaseResponse> GetTripTimeTableLocationAsync(int tripTimeTableId);
        Task<BaseResponse> GetCurrentTrip(string userId);
    }
}
