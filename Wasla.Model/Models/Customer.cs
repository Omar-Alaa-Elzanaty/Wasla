using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Customer:User
	{
		public int points { get; set; }
		public virtual ICollection<Reservation> Reservations { get; set; }
		public virtual ICollection<VehicleRate> VehicleRates { get; set; }
		public virtual ICollection<DriverRate> DriversRate { get; set; }
	}
}
