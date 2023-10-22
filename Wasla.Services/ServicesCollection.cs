using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Services.Initizalize;

namespace Wasla.Services
{
	public static class ServicesCollection
	{
		public static void AddServices(this IServiceCollection services) { 
			services.AddScoped<IInitializer,Initializer>();
		}
	}
}
