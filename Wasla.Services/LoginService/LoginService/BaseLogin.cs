using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess;
using Wasla.Model.Helpers;
using Wasla.Services.Exceptions;
using Wasla.Services.LoginService.ILoginService;

namespace Wasla.Services.LoginService.LoginService
{
    public class BaseLogin : IBaseLogin
    {
        private readonly BaseResponse _response;
        private readonly WaslaDb _db;
        private readonly IStringLocalizer<BaseLogin> _localization;

        public BaseLogin(WaslaDb db, IStringLocalizer<BaseLogin> localization)
        {
            _response = new();
            _db = db;
            _localization = localization;
        }

        public async Task<BaseResponse> LoginAsync(LoginHelp help)
        {
            _response.Message = _localization["LoginSuccess"].Value;
            switch (help.role)
            {
               case Roles.Role_Rider:
                    _response.Data =await (new LoginPassenger(_db)).Login(help);
                    break;
                case Roles.Role_Driver:
                    _response.Data = await (new LoginDriver(_db)).Login(help);
                    break;
                case Roles.Role_Organization:
                    _response.Data = await (new LoginOrganization(_db)).Login(help);
                    break;
                default:
                    throw new BadRequestException(_localization["roleNotFound"].Value);
            }
            return _response;
        }

     
    }
}
