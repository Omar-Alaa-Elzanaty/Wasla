using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Wasla.Model.Helpers;
using Wasla.Model.Models;

namespace Wasla.Model.Dtos
{
	public class EmployeeRegisterDto
	{
		public string FirstName { get;set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string Role { get; set; }
		public string ? Email { get; set; }
		public long NationalId { get; set; }
		public IFormFile? PhotoFile { get; set; }
		public Gender Gender { get; set; }
		public DateTime? Birthdate { get; set; }
	}
}
