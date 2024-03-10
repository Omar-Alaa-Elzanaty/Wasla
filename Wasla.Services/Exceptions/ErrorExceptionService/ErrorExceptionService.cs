using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wasla.Model.Helpers;
using Microsoft.AspNetCore.Mvc;
using Wasla.Model.Helpers.Statics;

namespace Wasla.Services.Exceptions.ErrorExceptionService
{
    public static class ErrorExceptionService
    {
      /*  public static BaseResponse GetErrorException(HttpStatusCode status, string message)
        {
            var response =  new BaseResponse();
            response.IsSuccess = false;
            response.Status = status;
            response.Message = message;
            return  response;
        }*/
    }
    /*  
          public Task GetError(int status, string message)
          {
              var response = new BaseResponse();
              response.IsSuccess = false;
              response.Status = (HttpStatusCode)status;
              response.Message = message;
              JsonSerializerOptions options = new JsonSerializerOptions
              {
                  PropertyNamingPolicy = JsonNamingPolicy.CamelCase
              };
              var exceptionResult = JsonSerializer.Serialize(response, options);

              _httpContextAccessor.HttpContext.Response.ContentType = "application/json";
              _httpContextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
              return _httpContextAccessor.HttpContext.Response.WriteAsync(exceptionResult);
          }
      }*/
}
