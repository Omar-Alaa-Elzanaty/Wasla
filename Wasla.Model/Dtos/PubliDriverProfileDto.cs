using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class PubliDriverProfileDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public Gender Gender { get; set; }
        public string LicenseImageUrl { get; set; }
        public DateTime? Birthdate { get; set; }
        public string LicenseNum { get; set; }
        public string? NationalId { get; set; }
        public PublicDriverProfileVehicleDto Vehicle { get; set; }
    }
    public class PublicDriverProfileVehicleDto
    {
        public int LicenseNumber { get; set; }
        public string LicenseWord { get; set; }
    }
}
