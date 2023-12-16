using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Net;
using System.Text.Json;
using Wasla.Model.Helpers;
using Wasla.Services.Exceptions;

namespace Wasla.Services.Middleware.ExceptionMiddleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStringLocalizer<GlobalExceptionHandlingMiddleware> _localization;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, IStringLocalizer<GlobalExceptionHandlingMiddleware> localization)
        {
            _next = next;
            _localization = localization;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {

                    var response = new BaseResponse();
                    response.IsSuccess = false;
                    response.Message =_localization["Unauthorized"].Value;
                    response.Status = HttpStatusCode.Unauthorized;

                    var exceptionResult = JsonSerializer.Serialize(response);
                    httpContext.Response.ContentType = "application/json";
                    httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    await httpContext.Response.WriteAsync(exceptionResult);
                    return;
                }
            }
            catch (Exception ex) 
            {
                await HandlingExceptionsAsync(httpContext,ex);
            }     
        }
        private static Task HandlingExceptionsAsync(HttpContext httpContext, Exception ex)
        {
            var response = new BaseResponse();
            response.IsSuccess = false;
            var exceptionType= ex.GetType();
            if(exceptionType == typeof(BadRequestException))
            {
                response.Message= ex.Message;
                response.Status=HttpStatusCode.BadRequest;  
            }
           else if (exceptionType == typeof(NotFoundException))
            {
                response.Message = ex.Message;
                response.Status= HttpStatusCode.NotFound;
            }
           else if (exceptionType == typeof(UnauthorizedException))
            {
                response.Message= ex.Message;
                response.Status = HttpStatusCode.Unauthorized;
            }
           else if (exceptionType == typeof(NotImplementeException))
            {
                response.Message= ex.Message;
                response.Status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(ForbiddenException))
            {
                response.Message = ex.Message;
                response.Status = HttpStatusCode.Forbidden;
            }
            else
            {
                response.Message = ex.Message;
                response.Status = HttpStatusCode.InternalServerError;
            }
            var exceptionResault = JsonSerializer.Serialize(response);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode=(int)HttpStatusCode.OK;
            return httpContext.Response.WriteAsync(exceptionResault);
        }
    }
}
