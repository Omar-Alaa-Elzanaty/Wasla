using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers.Statics
{
    public static class HttpStatusErrorCode
    {
        public const HttpStatusCode BadRequest = HttpStatusCode.BadRequest;
        public const HttpStatusCode Unauthorized = HttpStatusCode.Unauthorized;
        public const HttpStatusCode Forbidden = HttpStatusCode.Forbidden;
        public const HttpStatusCode NotFound = HttpStatusCode.NotFound;
        public const HttpStatusCode InternalServerError = HttpStatusCode.InternalServerError;
        public const HttpStatusCode NotImplemented = HttpStatusCode.NotImplemented;
        public const HttpStatusCode ServiceUnavailable = HttpStatusCode.ServiceUnavailable;
        public const HttpStatusCode NotExtended = HttpStatusCode.NotExtended;

    }
    /*
       BadRequest = HttpStatusCode.BadRequest,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        LoopDetected = 508,
        NotExtended = 510,
     */
}
