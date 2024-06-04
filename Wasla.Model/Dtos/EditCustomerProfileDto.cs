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
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? Photo { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthdate { get; set; }
    }
}
