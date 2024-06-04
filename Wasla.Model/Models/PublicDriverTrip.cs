using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;

namespace Wasla.Model.Models
{
    public class PublicDriverTrip
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TripStatus Status { get; set; }
        public bool AcceptRequests { get; set; }
        public bool AcceptPackages { get; set; }
        public int StartStationId { get; set;}
        public virtual Station StartStation { get; set; }
        public int EndStationId { get; set; }
        public virtual Station EndStation { get; set; }
        public string PublicDriverId { get; set; }
        public virtual PublicDriver PublicDriver { get; set; }
        public virtual List<PublicDriverTripRequest> Requests { get; set; }
        public int ReservedSeats { get; set; }
        public bool IsStart { get; set; }
        public bool IsActive { get; set; }
        public string? Latitude { get; set; }
        public string? Langtitude { get; set; }
    }
}
