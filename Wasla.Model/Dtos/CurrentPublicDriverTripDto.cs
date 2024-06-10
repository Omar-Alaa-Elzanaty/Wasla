using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class CurrentPublicDriverTripDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TripStatus Status { get; set; }
        public bool AcceptRequests { get; set; }
        public bool AcceptPackages { get; set; }
        public int StartStationId { get; set; }
        public virtual PublicStation StartStation { get; set; }
        public int EndStationId { get; set; }
        public virtual PublicStation EndStation { get; set; }
        public int ReservedSeats { get; set; }
        public bool IsStart { get; set; }
        public bool IsActive { get; set; }
        public string? Latitude { get; set; }
        public string? Langtitude { get; set; }
        public ICollection<PublicTripReservationDto> Reservations { get; set; }
        public ICollection<PublicTripPackagesRequestDto> PackagesRequests { get; set; }
    }
    public class PublicTripReservationDto
    {
        public int Id { get; set; }
        public bool OnRoad { get; set; }
        public PublicTripCustomerInfoDto Customer { get; set; }
    }
    public class PublicTripCustomerInfoDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public DateTime? Birthdate { get; set; }
    }
    public class PublicTripPackagesRequestDto
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
