using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class CurrentPublicDriverTripDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TripStatus Status { get; set; }
        public bool AcceptRequests { get; set; }
        public bool AcceptPackages { get; set; }
        public int StartStationId { get; set; }
        public virtual Station StartStation { get; set; }
        public int EndStationId { get; set; }
        public virtual Station EndStation { get; set; }
        public int ReservedSeats { get; set; }
        public bool IsStart { get; set; }
        public bool IsActive { get; set; }
        public string? Latitude { get; set; }
        public string? Langtitude { get; set; }
    }
}
