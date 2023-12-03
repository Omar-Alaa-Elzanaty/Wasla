using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
	public class AddVehicleDto
	{
		public string Category { get; set; }
		public int LicenseNumber { get; set; }
		public string LicenseWord { get; set; }
		public int Capcity { get; set; }
		public string Brand { get; set; }
		public float PackageCapcity { get; set; }
		public int AdsSidesNumber { get; set; }
		public IFormFile ImageFile { get; set; }
	}
}
