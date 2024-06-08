using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class AddOrganizationDriverDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public MediaFile? LicenseImageFile { get; set; }
        public string LicenseNumber { get; set; }
        public string PhoneNumber { get; set; }
    }
}
