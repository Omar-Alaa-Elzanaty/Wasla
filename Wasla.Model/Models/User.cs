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
		public string? PhotoUrl { get; set; } = "https://static.vecteezy.com/system/resources/thumbnails/009/292/244/small/default-avatar-icon-of-social-media-user-vector.jpg";

        public Gender Gender { get; set; }
		public DateTime? Birthdate { get; set; }

    }
    public  enum Gender:byte
	{
		Male=0,
		Female=1
	}
}
