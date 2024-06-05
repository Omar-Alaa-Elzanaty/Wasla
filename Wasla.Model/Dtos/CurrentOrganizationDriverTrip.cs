using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class CurrentOrganizationDriverTrip
    {
        public int Id { get; set; }
        public string DriverId { get; set; }
        public int VehicleId { get; set; }
        public int TripId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArriveTime { get; set; }
        public bool IsStart { get; set; }
        public TripStatus Status { get; set; }
        public TimeSpan BreakPeriod { get; set; }
        public float AvailablePackageSpace { get; set; }
        public string? Latitude { get; set; }
        public string? Langtitude { get; set; }
    }
}
