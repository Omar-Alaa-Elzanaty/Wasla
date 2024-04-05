using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class PubliDriverProfileDto
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime? Birthdate { get; set; }
        public string LicenseNum { get; set; }
        public string? NationalId { get; set; }
    }
    public class PublicDriverProfileVehicleDto
    {
        public int LicenseNumber { get; set; }
        public string LicenseWord { get; set; }
    }
}
