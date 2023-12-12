using Microsoft.Extensions.Localization;
using Wasla.Services.Exceptions.FilterException;
using Wasla.Services.AdminServices;
using Wasla.Services.Initizalize;
using Wasla.Services.MediaSerivces;
using Wasla.Services.Authentication.AuthServices;
using Wasla.Services.MultLanguageService.JsonLocalizer;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;
using Wasla.Services.Authentication.AuthHelperService.FactorService.Factory;
using Microsoft.Extensions.DependencyInjection;
using Wasla.Services.EmailServices;
using Wasla.Services.Middleware;
using Wasla.Services.OrganizationSerivces;
using Wasla.Services.PassangerServices;

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
            services.AddScoped<IOrganizationService,OrganizationSerivce>();
            services.AddScoped<IPassangerService, PassangerService>();
         
        }
    }
}
