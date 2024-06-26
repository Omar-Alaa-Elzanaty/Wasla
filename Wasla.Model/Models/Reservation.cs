﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Models
{
	public class Reservation
	{
		public int Id { get; set; }
		public int SetNum { get; set; }
		public string QrCodeUrl { get; set; }
		public DateTime ReservationDate { get; set; }
		public string CustomerId { get; set; }
		public virtual Customer Customer { get; set; }
		public int TripId { get; set; }
		public virtual Trip Trip { get; set; }
	}
}
