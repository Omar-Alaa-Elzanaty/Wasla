using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class GetAllOrgTripReservationDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsRide { get; set; }
        public string LocationDescription { get; set; }
        public bool OnRoad { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
