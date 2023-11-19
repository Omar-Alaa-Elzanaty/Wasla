﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class DataAuthResponse
    {
        public DataAuthResponse()
        {
            ConnectionData = new ConnectionData();
            TokensData = new TokensData();
        }

        public bool IsAuthenticated { get; set; }
        public ConnectionData ConnectionData { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public TokensData TokensData { get; set; }
      
    }
}