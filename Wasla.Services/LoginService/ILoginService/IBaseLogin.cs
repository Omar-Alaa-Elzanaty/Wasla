using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Services.LoginService.ILoginService
{
    public  interface IBaseLogin
    {
        Task<BaseResponse> LoginAsync(LoginHelp help);
    }
}
