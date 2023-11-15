using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Dtos;
using Wasla.Model.Helpers;

namespace Wasla.Services.LoginService.ILoginService
{
    public interface ILogin
    {
        Task<object> Login(LoginHelp loginHelp);
    }
}
