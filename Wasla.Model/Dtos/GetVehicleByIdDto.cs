using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class GetVehicleByIdDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int LicenseNumber { get; set; }
        public string LicenseWord { get; set; }
        public int Capcity { get; set; }
        public string Brand { get; set; }
        public float PackageCapcity { get; set; }
        public int AdsSidesNumber { get; set; }
        public string ImageUrl { get; set; }
        public string? PublicDriverId { get; set; }
        public string? OrganizationId { get; set; }
    }
}
