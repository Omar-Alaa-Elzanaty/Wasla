using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Driver:PublicDriver
	{
        public Driver()
        {
			Trips = new List<TripTimeTable>();
			Rates = new List<DriverRate>();
        }
        public string LicenseImageUrl { get; set; }
		public string LicenseNum { get; set; }
		public string? OrganizationId { get; set; }
		public virtual Organization? Orgainzation { get; set; }
		public string? NationalId { get; set; }
		public virtual ICollection<TripTimeTable> Trips { get; set; }
	}
}
