using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class PackageLocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string? ReciverUserName { get; set; }
        public string? ReciverName { get; set; }
        public string Latitude { get; set; }
        public string Langtitude { get; set; }
    }
}
