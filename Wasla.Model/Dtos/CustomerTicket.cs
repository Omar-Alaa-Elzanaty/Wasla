using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class CustomerTicket
    {
        public int TripId { get; set; }
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Price { get; set; }
        public int? SeatNumber { get; set; }
        public string PassengerName { get; set; }
        public bool IsPublic { get; set; }
    }
}
