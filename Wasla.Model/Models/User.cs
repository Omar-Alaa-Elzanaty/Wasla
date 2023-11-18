using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Model.Models
{
	public  class User:Account
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? PhotoUrl { get; set; }
		public Gender Gender { get; set; }
		public DateTime? Birthdate { get; set; }
		//public Account Account { get; set; }

    }
    public  enum Gender:byte
	{
		Male=0,
		Female=1
	}
}
