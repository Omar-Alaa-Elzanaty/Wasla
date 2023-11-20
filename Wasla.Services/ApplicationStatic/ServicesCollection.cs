using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Wasla.Services.AuthServices;
using Wasla.Services.Exceptions.FilterException;
using Wasla.DataAccess.AutoMapping;
using Wasla.Model.Helpers;
using Wasla.Model.Models;
using Wasla.Services.AdminServices;
using Wasla.Services.AuthServices;
using Wasla.Services.Exceptions.FilterException;
using Wasla.Services.Initizalize;
using Wasla.Services.MediaSerivces;
using Wasla.Services.Authentication.AuthServices;
using Microsoft.Extensions.Localization;
using Wasla.Services.MultLanguageService.JsonLocalizer;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;
using Wasla.Services.Authentication.AuthHelperService.FactorService.Factory;
using Wasla.Services.Exceptions.FilterException;

namespace Wasla.Services.ApplicationStatic
{
	public static class ServicesCollection
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
			services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IInitializer, Initializer>();
            //
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<IBaseFactoryResponse, BaseFactoryResponse>();



            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMediaSerivce,MediaService>();
            services.AddScoped<IMailServices, MailServices>();
         
        }
    }
}
