
using Wasla.Model.Helpers;

namespace Wasla.Services.Authentication.AdminServices
{
    public interface IAdminService
    {
        Task<BaseResponse> DisplayOrganiztionRequestsAsync();
        Task<BaseResponse> ConfirmOrgnaizationRequestAsync(int requestId);
    }
}
