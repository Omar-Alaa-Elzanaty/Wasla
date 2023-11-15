using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using Wasla.DataAccess;
using Wasla.DataAccess.AutoMapping;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.AdminServices;
using Wasla.Services.AuthServices;
using Wasla.Services.Exceptions.FilterException;
using Wasla.Services.Initizalize;
using Wasla.Services.MediaSerivces;
using Wasla.Services.LoginService.ILoginService;
using Wasla.Services.LoginService.LoginService;
using Wasla.Services.MultLanguageService.JsonLocalizer;

namespace Wasla.Services.ApplicationStatic
{
    public static class ServicesCollection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
			services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IInitializer, Initializer>();
            services.AddScoped<IBaseLogin,BaseLogin>();
            //
            services.AddScoped<ValidationFilterAttribute>();
         


            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMediaSerivces,MediaServices>();
         
        }
    }
}
