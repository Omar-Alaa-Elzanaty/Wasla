using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class PublicTriptReservationRequestDto
    {
        public string customerName { get; set; }
        public int CustomerReservationId { get; set; }
        public string TripTime { get; set; }
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public string? LocationDescription { get; set; }


    }
}
