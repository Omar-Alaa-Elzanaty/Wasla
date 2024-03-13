using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;

namespace Wasla.Model.Dtos
{
    public class DisplayCustomerProfileDto
    {
        public string UserName { get; set;}
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<Follow> Followers { get; set; } = new List<Follow>();
        public List<Follow> Following { get; set; } = new List<Follow>();
        public int Points { get; set; }

    }
    public class Follow
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
