using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class CreatePublicDriverCommand
    {
        public int StartStationId { get; set; }
        public int EndStationId { get; set;}
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        

    }
}
