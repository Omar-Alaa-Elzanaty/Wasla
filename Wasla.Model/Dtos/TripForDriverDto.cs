using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class TripForDriverDto
    {
        public float Price { get; set; }
        public TripForDriverVehicleDto Vehicle { get; set; }
        public float AvailablePackageSpace { get; set; }
        public TimeSpan Duration { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArriveTime { get; set; }
        public int AvailableSets { get; set; }
    }
    public class TripForDriverVehicleDto
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int LicenseNumber { get; set; }
        public string LicenseWord { get; set; }
        public int Capcity { get; set; }
        public string Brand { get; set; }
        public float PackageCapcity { get; set; }
        public int AdsSidesNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? OrganizationId { get; set; }
    }
}
