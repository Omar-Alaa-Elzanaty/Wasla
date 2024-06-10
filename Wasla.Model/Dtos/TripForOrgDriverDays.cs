using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class TripForOrgDriverDays
    {
        public int TripId { get; set; }
        public string TripStartTime { get; set; }
        public string TripEndTime { get; set; }
        public string TripDay { get; set;}
        public string TripDate { get; set;}
        public string StartStation { get; set;}
        public string EndStation { get; set;}
        public bool IsStart { get; set; }
    }
}
