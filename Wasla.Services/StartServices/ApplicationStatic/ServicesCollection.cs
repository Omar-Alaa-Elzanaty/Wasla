using Microsoft.Extensions.Localization;
using Wasla.Services.Exceptions.FilterException;
using Wasla.Services.Authentication.AuthServices;
using Wasla.Services.Authentication.AuthHelperService.FactorService.IFactory;
using Wasla.Services.Authentication.AuthHelperService.FactorService.Factory;
using Microsoft.Extensions.DependencyInjection;
using Wasla.Services.Authentication.VerifyService;
using Wasla.Services.ShareService.EmailServices;
using Wasla.Services.ShareService.AuthVerifyShareService;
using Wasla.Services.Authentication.AdminServices;
using Wasla.Services.StartServices.Initizalize;
using Wasla.Services.HlepServices.MediaSerivces;
using Wasla.Services.HlepServices.MultLanguageService.JsonLocalizer;
using Wasla.Services.EntitiesServices.OrganizationSerivces;
using Wasla.Services.EntitiesServices.PassangerServices;
using Wasla.Services.EntitiesServices.PublicDriverServices;

namespace Wasla.Services.StartServices.ApplicationStatic
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
            services.AddScoped<IMediaSerivce, MediaService>();
            services.AddScoped<IMailServices, MailServices>();
            services.AddScoped<IOrganizationService, OrganizationSerivce>();
            services.AddScoped<IPassangerService, PassangerService>();
            services.AddScoped<IDriverServices, DriverServices>();
            services.AddScoped<IVerifyService, VerifyService>();
            services.AddScoped<IAuthVerifyService, AuthVerifyService>();

        }
    }
}
