using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class UserFollow
	{
		public string UserId { get; set; }
		public virtual User User { get; set; }
		public string FollowerId { get; set; }
		public virtual User Follower { get; set; }
	}
}
