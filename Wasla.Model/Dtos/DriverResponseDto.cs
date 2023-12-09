using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class DriverResponseDto:BaseData
    {
        public IFormFile ProfileImageFile { get; set; }
        public IFormFile LicenseImageFile { get; set; }
        public string LicenseNum { get; set; }
        public virtual Organization? Orgainzation { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual ICollection<DriverRate> Rates { get; set; }
    }
}
