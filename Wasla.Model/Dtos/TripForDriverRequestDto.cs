using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class TripForDriverRequestDto
    {
        public string OrgId { get; set; }
        public string DriverId { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
