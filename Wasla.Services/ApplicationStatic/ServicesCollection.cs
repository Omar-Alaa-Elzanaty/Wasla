using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Wasla.Services.AdminServices;
using Wasla.Services.AuthServices;
using Wasla.Services.EmailServices;
using Wasla.Services.Initizalize;
using Wasla.Services.MediaSerivces;
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
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IMediaSerivce,MediaService>();
            services.AddScoped<IMailServices, MailServices>();
         
        }
    }
}
