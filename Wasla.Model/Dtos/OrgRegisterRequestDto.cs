﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
	public class OrgRegisterRequestDto
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string? PhoneNumber { get; set; }
		public IFormFile ImageFile { get; set; }
		public string? WebSiteLink { get; set; }
		public string Otp { get; set; }
	}
}
