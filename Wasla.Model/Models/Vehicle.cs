using System;
using System.Collections.Generic;
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
        }

        public int Id { get; set; }
		public string Type { get; set; }
		public int LicenseNumber { get; set; }
		public string LicenseWord { get; set; }
		public int Capcity { get; set; }
		public float PackageCapcity { get; set; }
		public string? OrganizationId { get; set; }
		public virtual Organization? Orgainzation { get; set; }
		public int? AdsId { get; set; }
		public virtual Advertisment? Advertisment { get; set; }
		public int? TripId { get; set; }
		public virtual Trip Trip { get; set; }
		public virtual ICollection<VehicleRate> Rates { get; set; }
	}
}
