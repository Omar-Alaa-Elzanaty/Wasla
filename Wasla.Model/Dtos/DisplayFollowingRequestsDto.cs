﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class DisplayFollowingRequestsDto
    {
        public string FollowingId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string? PhotoUrl { get; set; }
    }
}