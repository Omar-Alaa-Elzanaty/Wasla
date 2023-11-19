
using Wasla.Model.Helpers;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory
{
    public interface IBaseFactoryResponse
    {
        Task<BaseResponse> BaseAuthResponseAsync(AuthResponseFactoryHelp help);

    }
}
