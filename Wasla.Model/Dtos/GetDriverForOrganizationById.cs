using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
    public class GetDriverForOrganizationById
    {
        public string FullName { get; set; }
        public string LicenseImageUrl { get; set; }
        public string LicenseNum { get; set; }
        public string? NationalId { get; set; }
        public string? OrganizationId { get; set; }
        public string PhotoUrl { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
