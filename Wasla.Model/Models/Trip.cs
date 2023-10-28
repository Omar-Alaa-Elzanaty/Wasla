using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Trip
	{
		public int Id { get; set; }
		public string DriverId { get; set; }
		public virtual Driver Driver { get; set; }
		public int OrganizationId { get; set; }
		public virtual Organization Organization { get; set; }
		public int VehicleId { get; set; }
		public virtual Vehicle vehicle { get; set; }
		public DateTime LaunchTime { get; set; }
		public int		DurationTrip { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public int AvailableSets { get; set; }
		public float AvailablePackageSpace { get; set; }
		public virtual ICollection<Reservation> Reservations { get; set; }
		public virtual ICollection<Package> Packages { get; set; }
	}
}
