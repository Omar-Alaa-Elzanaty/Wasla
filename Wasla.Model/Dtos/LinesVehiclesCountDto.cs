using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class LinesVehiclesCountDto
    {
        public int LineId { get; set; }
        public string Strat { get; set; }
        public string End { get; set; }
        public int VehiclesCount { get; set; }
    }
}
