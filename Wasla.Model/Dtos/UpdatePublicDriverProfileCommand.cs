using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class UpdatePublicDriverProfileCommand
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string NationalId { get; set; }
        public string LicenseNum { get; set; }
    }
    public class UpdatePublicDriverProfileVehicleCommand
    {
        public int LicenseNumber { get; set; }
        public string LicenseWord { get; set; }
    }

}
