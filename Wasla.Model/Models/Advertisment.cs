using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Advertisment
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string ImageUrl { get; set; }
		public string organizationId { get; set; }
		public virtual ICollection<Vehicle> Busses { get; set; }
	}
}
