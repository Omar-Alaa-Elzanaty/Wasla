using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class TripDto
    {
        public OrgDriverDto Driver { get; set; }
        public float Price { get; set; }
    //    public Vehicle Vehicle { get; set; }
        public TimeSpan Duration { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int AvailableSets { get; set; }
        public float AvailablePackageSpace { get; set; }

    }
   
}
