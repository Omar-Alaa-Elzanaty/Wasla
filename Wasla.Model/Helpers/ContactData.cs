using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
    public class ContactData
    {
        public string Email { get; set; }
        public string phone { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public bool PhoneConfirmed { get; set; } = false;

    }
}
