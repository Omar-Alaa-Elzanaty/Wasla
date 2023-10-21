using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Driver:User
	{
		public int License { get; set; }
		public int? OrganizationId { get; set; }
		public virtual Organization? Orgainzation { get; set; }
		public virtual ICollection<Trip> Trips { get; set; }
	}
}
