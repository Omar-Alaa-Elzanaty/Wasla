using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Wasla.Model.Helpers;
using Microsoft.Extensions.Localization;

namespace Wasla.Services.Exceptions.FilterException
{
    public class ValidationFilterAttribute :IActionFilter
    {
        private readonly IStringLocalizer<ValidationFilterAttribute> _localization;

        public ValidationFilterAttribute(IStringLocalizer<ValidationFilterAttribute> localization)
        {
            _localization = localization;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var httpContext = context.HttpContext;
                httpContext.Response.ContentType = "application/json";
                var errorResponse = new BaseResponse
                {
                    ErrorMessages = context.ModelState.Values
                     .SelectMany(v => v.Errors)
                     .Select(e => e.ErrorMessage).ToList().FirstOrDefault(),
                    Result = null,
                    HttpStatusCode = HttpStatusCode.BadRequest,
                    isSuccess = false
                    
                };
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    
                };
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
        /*  public void OnActionExecuting(ActionExecutingContext context)
          {
              if (!context.ModelState.IsValid)
              {
                  context.Result =  new UnprocessableEntityObjectResult(context.ModelState);
              }
          }
          public void OnActionExecuted(ActionExecutedContext context) { }*/
    }
}
