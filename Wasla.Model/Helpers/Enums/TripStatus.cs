﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers.Enums
{
    public enum TripStatus:byte
    {
        None=0,
        OnRoad,
        Waiting,
        Arrived,
        end,
        TakeBreak
    }
}

