using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class UpdateOrgDriverInfoDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public IFormFile? Photo { get; set; }
        public DateTime? BirthDate { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? LicenseImage { get; set; }
        public string LicenseNum { get; set; }
        public string? NationalId { get; set; }
    }
}
