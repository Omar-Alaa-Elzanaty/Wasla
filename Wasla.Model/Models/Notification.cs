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
        public virtual Account Account { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationType Type { get; set; }
    }
}
