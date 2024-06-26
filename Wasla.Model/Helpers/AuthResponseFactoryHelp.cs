﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class AuthResponseFactoryHelp
    {
        public AuthResponseFactoryHelp()
        {
            TokensData = new AuthData();
        }

        public AuthData TokensData { get; set; }
        public string userId { get; set; }
        public string role { get; set; }
    }
}
