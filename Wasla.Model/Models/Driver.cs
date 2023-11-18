using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Driver:User
	{
        public Driver()
        {
			Trips = new List<Trip>();
			Rates = new List<DriverRate>();
        }
        public string LicenseImageUrl { get; set; }
		public int LicenseNum { get; set; }
		public string? OrganizationId { get; set; }
		public virtual Organization? Orgainzation { get; set; }
		public virtual ICollection<Trip> Trips { get; set; }
		public virtual ICollection<DriverRate> Rates { get; set; }
	}
}
