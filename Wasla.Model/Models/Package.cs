using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Package
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string SenderId { get; set; }
		public string ImageUrl { get; set; }
		public string Price { get; set; }
		public float Weight { get; set; }
		public string? ReciverUserName { get; set; }
		public string? ReciverName { get; set; }
		public string? ReciverPhoneNumber { get; set; }
		public int? TripId { get; set; }
		public virtual Trip? Trip { get; set; }
	}
}
