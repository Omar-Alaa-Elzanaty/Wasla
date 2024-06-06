using Wasla.Model.Helpers.Enums;

namespace Wasla.Model.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SenderId { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public float Weight { get; set; }
        public string? ReciverUserName { get; set; }
        public string? ReciverName { get; set; }
        public string ReciverPhoneNumber { get; set; }
        public int? TripId { get; set; }
        public virtual TripTimeTable? Trip { get; set; }
        public string? DriverId { get; set; }
        public virtual PublicDriver? Driver { get; set; }
        public PackageStatus Status { get; set; }
    }
}
