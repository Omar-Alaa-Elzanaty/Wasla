using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class PassengerPublicTripRequestDto
    {
        public bool OnRoad { get; set; }
        public string LocationDescription { get; set; }
        public int PublicDriverTripId { get; set; }
    }
}
