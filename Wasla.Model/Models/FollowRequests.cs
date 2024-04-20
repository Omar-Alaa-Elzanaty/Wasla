using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
    public class FollowRequests
    {
        public string SenderId { get; set; }
        public virtual Customer Sender { get; set; }
        public string FollowerId { get; set; }
        public virtual Customer Follower { get; set; }
    }
}
