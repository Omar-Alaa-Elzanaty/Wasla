using System.ComponentModel.DataAnnotations;

namespace Wasla.Model.Models
{
	public class OrganizationRegisterRequest
	{
		[Key]
		public int RequestId { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string LogoUrl { get; set; }
		public string? WebSiteLink {  get; set; }
	}
}
