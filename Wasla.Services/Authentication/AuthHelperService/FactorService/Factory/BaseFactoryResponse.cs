using Microsoft.AspNetCore.Identity;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        public BaseFactoryResponse(WaslaDb db, IStringLocalizer<BaseFactoryResponse> localization,RoleManager<IdentityRole> roleManager)
        {
            _response = new();
            _db = db;
            _localization = localization;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse> BaseAuthResponseAsync(AuthResponseFactoryHelp help,string role)
        {
            // _response.Message = _localization["LoginSuccess"].Value;
            switch (role)
            {
                case Roles.Role_Passenger:
                    _response.Data = await new PassengerResponse(_db).AuthRespnseFactory(help);
                    break;
                case Roles.Role_Driver:
                    _response.Data = await new DriverResponse(_db,_roleManager).AuthRespnseFactory(help);
                    break;
                case Roles.Role_Org_SuperAdmin:
                    _response.Data = await new OrganizationResponse(_db,_roleManager).AuthRespnseFactory(help);
                    break;
                case Roles.Role_Org_Employee:
                    _response.Data = await new OrgEmployeeResponse(_db, _roleManager).AuthRespnseFactory(help);
                    break;
                default:
                    throw new BadRequestException(_localization["roleNotFound"].Value);
            }
            return _response;
        }
    }
}
