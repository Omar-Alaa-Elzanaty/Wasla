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
        public User()
        {
			PhotoUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fstock.adobe.com%" +
						"2Fsearch%3Fk%3Dmy%2Bprofile%2Bicon&psig=AOvVaw1qhmyXH-T4qu9HSzWFXi63&ust=1700466263740000" +
						"&source=images&cd=vfe&ved=0CBIQjRxqFwoTCKjHtM7Iz4IDFQAAAAAdAAAAABAE";
		}
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
