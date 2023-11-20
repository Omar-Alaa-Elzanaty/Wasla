
using Wasla.Model.Helpers;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory
{
    public interface IAuthResponse
    {
        Task<DataAuthResponse> AuthRespnseFactory(AuthResponseFactoryHelp loginHelp);
    }
}
