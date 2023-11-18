using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Organization:Account
	{
		public Organization() {
			TripList = new List<Trip>();
		}
		public string Name { get; set; }
		public string Address { get; set; }
		public string LogoUrl { get; set; }
		public float MaxWeight { get; set; }
		public float MinWeight { get; set; }
		public string? WebsiteLink { get; set; }
		public virtual ICollection<Trip> TripList { get; set; }
	}
}
