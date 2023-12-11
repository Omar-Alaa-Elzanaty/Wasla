using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Employee:User
	{
		public long NationalId { get; set; }
		public string OrgId { get; set; }
		public virtual Organization Organization { get; set; }
    }
}
