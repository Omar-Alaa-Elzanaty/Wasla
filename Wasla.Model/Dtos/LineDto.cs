﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class LineDto
    {
        public virtual Station Start { get; set; }
        public virtual Station End { get; set; }
    }
}
