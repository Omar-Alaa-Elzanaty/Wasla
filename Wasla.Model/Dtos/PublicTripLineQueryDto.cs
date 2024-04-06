using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class PublicTripLineQueryDto
    {
        public string StartStation { get; set; }
        public string StartLangtitude { get; set; }
        public string StartLatitude { get; set; }
        public string EndStation { get; set; }
        public string EndLangtitude { get; set; }
        public string EndLatitude { get; set; }
    }
}
