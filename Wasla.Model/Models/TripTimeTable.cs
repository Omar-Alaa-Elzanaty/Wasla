using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;

namespace Wasla.Model.Models
{
    public class TripTimeTable
    {
        public int Id { get; set; }
        public string DriverId { get; set; }
        public virtual Driver Driver { get; set; }
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual int TripId {  get; set; }
        public virtual Trip Trip { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArriveTime { get; set; }
        public bool IsStart { get; set; }
        public TripSatus Status { get; set; }
        public TimeSpan BreakPeriod { get; set; }
        public virtual List<Seat> RecervedSeats { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Package> Packages { get; set; }
        public float AvailablePackageSpace { get; set; }

    }
    public class Seat
    {
        public int setNum { get; set; }
        public int TripTmeTableId { get; set; }
    }
}
