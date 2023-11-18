using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
	public class DriverRegisterDto
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public IFormFile ProfileImageFile { get; set; }
		public IFormFile LicenseImageFile { get; set; }
		public int LicenseNum { get; set; }
		public DateTime BirthDate { get; set; }
		public Gender Gender { get; set; }
	}
}
