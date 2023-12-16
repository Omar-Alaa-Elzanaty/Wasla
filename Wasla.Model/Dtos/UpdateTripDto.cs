
namespace Wasla.Model.Dtos
{
    public class UpdateTripDto
    {
        public string DriverId { get; set; }
        public string orgId { get; set; }
        public float Price { get; set; }
        public int VehicleId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
       public DateTime launchingTime { get; set; }
       public DateTime arrivingTime { get; set; }
    }
}
