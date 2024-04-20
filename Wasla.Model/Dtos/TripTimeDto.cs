using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class TripTimeDto
    {
        public int Id { get; set; }
        // public virtual Driver Driver { get; set; }=new Driver();
        //  public virtual Vehicle Vehicle { get; set; } = new Vehicle();
        // public virtual Trip Trip { get; set; } = new Trip();
       //trip
        public virtual Line Line { get; set; } = new Line();
        public float Price { get; set; }
        public TimeSpan Duration { get; set; }
        public int Points { get; set; }
        public TripStatus Status { get; set; }

        //
        //vehicle

        public string Category { get; set; }
        public string Brand { get; set; }
        //
        //driver
        public string DriverName { get; set; }
        //
        public float PackageCapcity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArriveTime { get; set; }
        public float AvailablePackageSpace { get; set; }
        public bool IsStart { get; set; }
    }
}
