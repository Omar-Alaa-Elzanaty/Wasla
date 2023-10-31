using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class CustomerTripOrder
	{
		public int Id { get; set; }
		public string OrganizationId { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public string CustomerId { get; set; }
		public int RequiredSeats { get; set; }
		public float RequiredPackagesWeight { get; set; }
		public float Price { get; set; }
		public DateTime CreationDate { get; set; }
		public int VehicleId { get; set; }
		public State OrderState { get; set; }
	}
	public enum State:byte
	{
		NotConfirmed=0,
		Confirmed=1,
		Hold=2,
		Cancelled=3,
	}
}
