﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class LoginHelp
    {
        public LoginHelp()
        {
            TokensData = new TokensData();
        }

        public TokensData TokensData { get; set; }
        public string userId { get; set; }
        public string role { get; set; }
    }
}
