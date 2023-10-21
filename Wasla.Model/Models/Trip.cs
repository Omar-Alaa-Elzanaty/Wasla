using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Trip
	{
		public int Id { get; set; }
		public int DrvierId { get; set; }
		public virtual Driver Driver { get; set; }
		public int OrganizationId { get; set; }
		public virtual Orgainzation Orgainzation { get; set; }
		public int VechilesId { get; set; }
		public virtual Vechiles Vechiles { get; set; }
		public DateTime LaunchTime { get; set; }
		public DateTime ArriveTime { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public int AvailableSets { get; set; }
		public float AvailablePackageSpace { get; set; }
	}
}
