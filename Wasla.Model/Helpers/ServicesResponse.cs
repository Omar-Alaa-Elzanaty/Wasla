using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Helpers
{
	public class ServicesResponse<T>
	{
		public bool IsSuccess { get; set; }
		public string Log { get; set; }
		public T Entity { get; set; }
	}
}
