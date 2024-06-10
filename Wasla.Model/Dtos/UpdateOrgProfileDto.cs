using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class UpdateOrgProfileDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public float MaxWeight { get; set; }
        public float MinWeight { get; set; }
        public string WebsiteLink { get; set; }
        public IFormFile Logo { get; set; }

      
    }
}
