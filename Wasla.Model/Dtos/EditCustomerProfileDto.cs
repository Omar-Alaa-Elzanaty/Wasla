using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class EditCustomerProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get;set; }
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public IFormFile? Photo { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
