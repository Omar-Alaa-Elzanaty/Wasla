using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Wasla.Model.Helpers;

namespace Wasla.Services.Exceptions.FilterException
{
    public class ValidationFilterAttribute :IActionFilter
    {
        public ValidationFilterAttribute()
        {
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors= context.ModelState.Values.Select(v=>v.Errors).ToList();
                var httpContext = context.HttpContext;
                httpContext.Response.ContentType = "application/json";
                var errorResponse = new BaseResponse
                {
                    Message = context.ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage).ToList().FirstOrDefault(),
                    Data = null,
                    Status = HttpStatusCode.BadRequest,
                    IsSuccess = false
                };
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    
                };
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
