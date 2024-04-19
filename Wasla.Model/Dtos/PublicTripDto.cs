using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class PublicTripDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TripSatus Status { get; set; }
        public int StartStationId { get; set; }
        public  string StartStation { get; set; }
        public  string EndStation { get; set; }
        public string PublicDriverName { get; set; }
        public int ReservedSeats { get; set; }
    }
}
