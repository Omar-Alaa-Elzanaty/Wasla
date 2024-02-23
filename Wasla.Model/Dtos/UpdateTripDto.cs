
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class UpdateTripDto
    {
        public int LineId { get; set; }
        public float Price { get; set; }
        public int Duration { get; set; }
        public int Points { get; set; }
        public bool IsPublic { get; set; }
        public float AdsPrice { get; set; }
      
    }
}
