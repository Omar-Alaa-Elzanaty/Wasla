using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Customer:User
	{
		public Customer()
		{
			Reservations = new List<Reservation>();
			VehicleRates = new List<VehicleRate>();
			DriversRate = new List<DriverRate>();
			OrganizationRates = new List<OrganizationRate>();
		}

		public int points { get; set; }
		public virtual ICollection<Reservation> Reservations { get; set; }
		public virtual ICollection<VehicleRate> VehicleRates { get; set; }
		public virtual ICollection<DriverRate> DriversRate { get; set; }
		public virtual ICollection<OrganizationRate> OrganizationRates { get; set; }

	}
}
