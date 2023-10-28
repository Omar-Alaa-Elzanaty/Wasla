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
	public abstract class User:IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string? PhotoUrl { get; set; }
		public bool Gender { get; set; }
		public DateTime? Birthdate { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

    }
    public  struct Gender
	{
		public static readonly bool Male=true;
		public static readonly bool Female=false;
    }
}
