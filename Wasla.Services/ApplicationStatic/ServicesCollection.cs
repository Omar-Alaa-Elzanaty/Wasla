using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Services.MultLanguageService.JsonLocalizer;

namespace Wasla.Services.ApplicationStatic
{
    public static class ServicesCollection
    {
        public static void AddServices(this IServiceCollection services)
        {
			services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
		}
    }
}
