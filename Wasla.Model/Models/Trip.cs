using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace Wasla.Model.Models
{
	public class Trip
	{
		public int Id { get; set; }
		public string OrganizationId { get; set; }
		public virtual Organization Organization { get; set; }
        public int LineId { get; set; }
        public virtual Line Line { get; set; }
        public float Price { get; set; }
		public TimeSpan Duration { get; set; }
		public int Points { get; set; }
		public bool IsPublic { get; set; }
		public float AdsPrice { get; set; }
		public virtual List<TripTimeTable> TimesTable { get; set; }
		public Trip()
		{
		}
	}
}
