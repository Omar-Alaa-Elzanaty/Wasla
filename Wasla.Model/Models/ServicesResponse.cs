using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class ServicesResponse<T>
	{
		public bool State { get; set; }
		public T Service { get; set; }
		public string Comment { get; set; }
	}
}
