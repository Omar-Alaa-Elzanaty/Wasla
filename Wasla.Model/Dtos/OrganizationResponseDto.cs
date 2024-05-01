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
        public List<string> OrgPermissions { get; set; }
        public virtual ICollection<OrganizationTripResponse> TripList { get; set; }
    }
    public class OrganizationTripResponse
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public float Price { get; set; }
        public TimeSpan Duration { get; set; }
        public int Points { get; set; }
        public bool IsPublic { get; set; }
        public float AdsPrice { get; set; }
    }
}
