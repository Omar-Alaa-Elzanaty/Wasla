using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers;

namespace Wasla.Model.Models
{
	public  class Account:IdentityUser
	{
		public List<RefreshToken> RefreshTokens { get; set; }
	}
}
