﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class GetAllNotificationsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string NotifactionImage { get; set; }
        public string NotifactionName { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime NotificationTime { get; set; }

    }
}
