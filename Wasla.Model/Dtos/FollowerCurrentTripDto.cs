using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class FollowerCurrentTripDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string CustomerImageUrl { get; set; }
        public string CompanyImageUrl { get; set; }
        public string StartLangtitude { get; set; }
        public string StartLatitude { get; set; }
        public string EndLangtitude { get; set; }
        public string EndLatitude { get; set; }
        public string FullName { get; set; }
        public string Latitude { get; set; }
        public string Langtitude { get; set; }
    }
}
