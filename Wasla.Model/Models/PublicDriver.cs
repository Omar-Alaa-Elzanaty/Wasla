﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
    public class PublicDriver:User
    {
        public string LicenseImageUrl { get; set; }
        public string LicenseNum { get; set; }
        public string? NationalId { get; set; }
        public int StartId { get; set; }
        public virtual PublicStation StartStation { get; set; }
        public int EndId { get; set; }
        public bool isActive { get; set; } = false;
        public virtual PublicStation EndStation { get; set; }
        public virtual ICollection<PublicDriverRate> Rates { get; set; }
    }
}
