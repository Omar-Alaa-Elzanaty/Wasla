using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
	public class DriverRegisterDto:RegisterHelp
	{
		[Required(ErrorMessage = "ProfileImageFileRequire")]
		public IFormFile? ProfileImageFile { get; set; }
        [Required(ErrorMessage = "LicenseImageFileRequire")]

        public IFormFile? LicenseImageFile { get; set; }
        [Required(ErrorMessage = "LicenseNumRequire")]

        public string LicenseNum { get; set; }
        [Required(ErrorMessage ="startIdRequired")]
        public int StartId { get; set; }
        [Required(ErrorMessage = "endIdRequired")]

        public int EndId { get; set; }

    }
}
