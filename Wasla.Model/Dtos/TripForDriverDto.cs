using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class TripForDriverDto
    {
        public float Price { get; set; }
        public Vehicle Vehicle { get; set; }
        public float AvailablePackageSpace { get; set; }
        public TimeSpan Duration { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int AvailableSets { get; set; }
    }
}
