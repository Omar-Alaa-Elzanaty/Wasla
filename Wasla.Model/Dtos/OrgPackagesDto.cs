using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class OrgPackagesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SenderId { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string? ReciverUserName { get; set; }
        public string? ReciverName { get; set; }
        public string ReciverPhoneNumber { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public TimeSpan Duration { get; set; }
        //
        //vehicle
        public string VehicleCategory { get; set; }
        public string VehicleBrand { get; set; }
        //
        //driver
        public string DriverName { get; set; }
        //
        public DateTime TripStartTime { get; set; }
        public DateTime TripArriveTime { get; set; }
        public bool IsStart { get; set; }

        public string  Status { get; set; }
        public bool IsPublic => false;
    }
}
