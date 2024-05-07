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
			Rates = new List<DriverRate>();
        }
        public string LicenseImageUrl { get; set; }
        public string LicenseNum { get; set; }
        public string? NationalId { get; set; }
        public string? OrganizationId { get; set; }
		public virtual Organization? Orgainzation { get; set; }
        public virtual ICollection<DriverRate> Rates { get; set; }
    }
}
