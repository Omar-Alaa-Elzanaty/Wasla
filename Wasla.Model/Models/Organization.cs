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
			Stations = new List<Station>();
			Drivers=new List<Driver>();
			Employees=new List<Employee>();
			Vehicles=new List<Vehicle>();
			Rates = new List<OrganizationRate>();
		}
		public string Name { get; set; }
		public string Address { get; set; }
		public string? LogoUrl { get; set; }
		public float MaxWeight { get; set; }
		public float MinWeight { get; set; }
		public string? WebsiteLink { get; set; }
		public virtual List<Driver> Drivers { get; set; }
		public virtual ICollection<Trip> TripList { get; set; }
		public virtual ICollection<Station>Stations { get; set; }
		public virtual ICollection<Employee> Employees { get; set; }
		public virtual ICollection<Vehicle> Vehicles { get; set; }
		public virtual ICollection<OrganizationRate> Rates { get; set; }
	}
}
