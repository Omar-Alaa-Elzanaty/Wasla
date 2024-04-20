using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;

namespace Wasla.Model.Dtos
{
    public class UpdateOrgTripStatusCommand
    {
        public int TripTimeTableId { get; set; }
        public TripStatus Status { get; set; }
    }
}
