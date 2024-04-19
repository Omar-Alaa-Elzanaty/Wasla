using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;

namespace Wasla.Model.Dtos
{
    public class PublicTripStatusQueryDto
    {
        public TripSatus Status { get; set; }
        public int TotalTripSeats { get; set; }
        public int ReserverdSeats { get; set; }
    }
}
