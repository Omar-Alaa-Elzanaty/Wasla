using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public enum Rate : byte
	{
		VeryBad,
		Bad,
		Normal,
		Good,
		VeryGood
	}
	public class VehicleRate
	{
		public int VehicleId { get; set; }
		public virtual Vehicle Vehicle { get; set; }
		public string CustomerId { get; set; }
		public virtual Customer Customer { get; set; }
		public Rate Rate { get; set; }
	}
	public class DriverRate
	{
		public string DriverId { get; set; }
		public virtual Driver Driver { get; set; }
		public string CustomerId { get; set; }
		public virtual Customer Customer { get; set; }
		public Rate Rate { get; set; }
	}
}
