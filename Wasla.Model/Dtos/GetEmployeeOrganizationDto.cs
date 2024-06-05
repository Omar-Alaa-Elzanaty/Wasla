using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class GetEmployeeOrganizationDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public Gender Gender { get; set; }
        public long NationalId { get; set; }
        public string OrgId { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
