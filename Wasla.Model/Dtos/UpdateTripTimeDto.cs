using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class UpdateTripTimeDto
    {
        public string DriverId { get; set; }
        public int VehicleId { get; set; }
        public virtual int TripId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArriveTime { get; set; }
        [JsonIgnore]
        public byte Status { get; set; } =(byte) TripStatus.Arrived;
        public float AvailablePackageSpace { get; set; }
        public bool IsStart { get; set; }
    }
}
