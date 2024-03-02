using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class BaseResponse
    {
        public HttpStatusCode Status { get; set; }=HttpStatusCode.OK;
		public bool IsSuccess { get; set; } = true;
		public string? Message { get; set; }
		public object? Data { get; set; }
        public static BaseResponse GetErrorException(HttpStatusCode status, string message)
        {
            var response = new BaseResponse();
            response.IsSuccess = false;
            response.Status = status;
            response.Message = message;
            return response;
        }
    }
}
