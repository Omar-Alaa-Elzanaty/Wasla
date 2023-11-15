using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class OrganizationResponseDto:DataAuthResponse
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string LogoUrl { get; set; }
        public float MaxWeight { get; set; }
        public float MinWeight { get; set; }
        public string WebsiteLink { get; set; }

        public virtual ICollection<Trip> TripList { get; set; }
    }
}
