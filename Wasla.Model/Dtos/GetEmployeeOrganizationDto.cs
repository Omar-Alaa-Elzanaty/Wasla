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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhotoUrl { get; set; }
        public Gender Gender { get; set; }
        public long NationalId { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
