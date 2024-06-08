using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class GetOrgDriverProfileDto
    {
        public string Id { get; set; }
        public string? LicenseImageUrl { get; set; }
        public string LicenseNum { get; set; }
        public string? NationalId { get; set; }
        public string? OrganizationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
