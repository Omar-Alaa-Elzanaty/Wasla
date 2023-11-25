using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
	public class DriverRegisterDto:RegisterHelp
	{
		[Required(ErrorMessage = "ProfileImageFileRequire")]
		public IFormFile ProfileImageFile { get; set; }
        [Required(ErrorMessage = "LicenseImageFileRequire")]

        public IFormFile LicenseImageFile { get; set; }
        [Required(ErrorMessage = "LicenseNumRequire")]

        public int LicenseNum { get; set; }
		/*public DateTime BirthDate { get; set; }
		public Gender Gender { get; set; }*/
	}
}
