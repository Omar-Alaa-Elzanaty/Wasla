using Wasla.Model.Dtos;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Services.EntitiesServices.PassangerServices
{
    public interface IPassangerService
    {
        Task<BaseResponse> ReservationAsync(ReservationDto order);
        Task<BaseResponse> SeatsRecordsAsync(int tripId);
        Task<BaseResponse> OrganizationRateAsync(OrganizationRate model);
        Task<BaseResponse> OrganizationRateRemoveAsync(string organizationId, string customerId);
    }
}
