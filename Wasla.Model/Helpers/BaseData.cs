﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class BaseData:DataAuthResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
