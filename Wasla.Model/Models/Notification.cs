using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Helpers.Enums;

namespace Wasla.Model.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string NotifactionImage { get; set; }
        public string NotifactionName { get; set; }
        public virtual Account Account { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
    }
}
