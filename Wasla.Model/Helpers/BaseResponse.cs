﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class BaseResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }=HttpStatusCode.OK;
        public bool isSuccess { get; set; } = true;
        public  string? ErrorMessages { get; set; }
       // public List<string>? ErrorMessags { get; set; }

        public object? Result { get; set; }
    }
}