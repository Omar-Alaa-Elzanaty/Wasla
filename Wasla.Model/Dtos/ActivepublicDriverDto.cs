using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class ActivepublicDriverDto
    {
        public string DriverName { get; set; }
        public string StartStation { get; set; }
        public string EndStation { get; set; }
        public bool AcceptPackages { get; set; }
        public bool AcceptReservationRequest { get; set; }
    }
}
