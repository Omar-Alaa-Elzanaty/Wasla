

using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class AddTripDto
    {
        public string DriverId { get; set; }
        public float Price { get; set; }
        public int VehicleId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
