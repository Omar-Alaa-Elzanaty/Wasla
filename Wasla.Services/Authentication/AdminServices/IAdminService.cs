
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.Authentication.AdminServices
{
    public interface IAdminService
    {
        Task<BaseResponse> DisplayOrganiztionRequestsAsync();
        Task<BaseResponse> ConfirmOrgnaizationRequestAsync(int requestId);
        Task<BaseResponse> GetAllOrgsAsync();
        Task<BaseResponse> DisplayOrganizationRequestAsync();
        Task<BaseResponse> AddStationAsync(StationDto model);
        Task<BaseResponse> UpdateStationAsync(StationDto stationDto,int stationId);
        Task<BaseResponse> GetStationsAsync();
        Task<BaseResponse> GetStationAsync(int id);
        Task<BaseResponse> DeleteStationAsync(int id);
        Task<BaseResponse> GetActiveDrivers(string startStation,string endStation);
    }
}
