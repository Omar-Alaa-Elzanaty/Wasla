using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class DriverPackagesDto
    {
        public string Name { get; set; }
        public string SenderId { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string? ReciverUserName { get; set; }
        public string? ReciverName { get; set; }
        public string ReciverPhoneNumber { get; set; }
        public string Status { get; set; }
    }
}
