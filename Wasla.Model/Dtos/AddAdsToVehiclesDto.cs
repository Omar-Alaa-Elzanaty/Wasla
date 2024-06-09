using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
    public class AddAdsToVehiclesDto
    {
        public int AdsId { get; set; }
        public List<int> Buses { get; set; }
    }
}
