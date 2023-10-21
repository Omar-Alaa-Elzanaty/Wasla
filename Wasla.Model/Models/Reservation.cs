using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Reservation
	{
		public int Id { get; set; }
		public DateTime ReservationDate { get; set; }
		public string UserId { get; set; }
		public User User { get; set; }
		public int TripId { get; set; }
		public virtual Trip Trip { get; set; }
	}
}
