using Microsoft.Extensions.Localization;
using Wasla.DataAccess;
using Wasla.Model.Helpers;
using Wasla.Model.Helpers.Statics;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;
using Wasla.Services.Exceptions;

namespace Wasla.Services.Authentication.AuthHelperService.FactorService.Factory
{
    public class BaseFactoryResponse : IBaseFactoryResponse
    {
        private readonly BaseResponse _response;
        private readonly WaslaDb _db;
        private readonly IStringLocalizer<BaseFactoryResponse> _localization;

        public BaseFactoryResponse(WaslaDb db, IStringLocalizer<BaseFactoryResponse> localization)
        {
            _response = new();
            _db = db;
            _localization = localization;
        }

        public async Task<BaseResponse> BaseAuthResponseAsync(AuthResponseFactoryHelp help)
        {
            // _response.Message = _localization["LoginSuccess"].Value;
            switch (help.role)
            {
                case Roles.Role_Rider:
                    _response.Data = await (new PassengerResponse(_db)).AuthRespnseFactory(help);
                    break;
                case Roles.Role_Driver:
                    _response.Data = await (new DriverResponse(_db)).AuthRespnseFactory(help);
                    break;
                case Roles.Role_Organization:
                    _response.Data = await (new OrganizationResponse(_db)).AuthRespnseFactory(help);
                    break;
                default:
                    throw new BadRequestException(_localization["roleNotFound"].Value);
            }
            return _response;
        }
    }
}
