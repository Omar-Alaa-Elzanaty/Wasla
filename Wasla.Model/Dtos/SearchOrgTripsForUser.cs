using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class SearchOrgTripsForUser: SearchTripsForUser
    {
        public float Price { get; set; }
        public string OrgName { get; set; }

        public ICollection<OrganizationRate> Rates { get; set; }
    }
}
