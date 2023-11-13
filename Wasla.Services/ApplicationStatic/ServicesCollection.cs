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
using Wasla.Services.AuthService;
using Wasla.Services.Exceptions.FilterException;
using Wasla.Services.Initizalize;
using Wasla.Services.MultLanguageService.JsonLocalizer;

namespace Wasla.Services.ApplicationStatic
{
    public static class ServicesCollection
    {
        public static void AddServices(this IServiceCollection services)
        {
            

            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();


            services.AddControllers();
            services.AddAutoMapper(typeof(AuthAutoMapper));
            services.AddScoped<IInitializer, Initializer>();

            //
            services.AddScoped<ValidationFilterAttribute>();
         


        }
    }
}
