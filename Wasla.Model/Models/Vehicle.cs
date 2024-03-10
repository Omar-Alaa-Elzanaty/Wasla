using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Vehicle
	{
        public Vehicle()
        {
            Rates=new List<VehicleRate>();
			Advertisment = new List<Advertisment>();
        }

        public int Id { get; set; }
		public string Category { get; set; }
		public int LicenseNumber { get; set; }
		public string LicenseWord { get; set; }
		public int Capcity { get; set; }
		public string Brand { get; set; }
		public float PackageCapcity { get; set; }
		public int AdsSidesNumber { get; set; }
		public string ImageUrl { get; set; }
		public string? OrganizationId { get; set; }
		public virtual List<Advertisment> Advertisment { get; set; }
		public virtual List<TripTimeTable> Trips { get; set; }
		public virtual ICollection<VehicleRate> Rates { get; set; }
	}
	
}
