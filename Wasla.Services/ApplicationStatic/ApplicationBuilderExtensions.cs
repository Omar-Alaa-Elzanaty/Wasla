using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Services.Middleware.ExceptionMiddleware;

namespace Wasla.Services.ApplicationStatic
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddGlobalExceptionGlobalHandler(this IApplicationBuilder app)
     =>app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
