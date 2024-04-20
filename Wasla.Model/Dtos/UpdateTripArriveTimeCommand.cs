using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class UpdateTripArriveTimeCommand
    {
        public DateTime ArriveTime { get; set; }
        public int TripTimeTableId { get; set; }

    }
}
