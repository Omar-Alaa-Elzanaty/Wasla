using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class UserFollow
	{
		public string CustomerId { get; set; }
		public virtual Customer Customer { get; set; }
		public string FollowerId { get; set; }
		public virtual Customer Follower { get; set; }
	}
}
