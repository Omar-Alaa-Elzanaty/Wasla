using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasla.Model.Dtos
{
	public class ResevrationDto
	{
		public string CustomerId { get; set; }
		public int TripId { get; set; } 
		public List<SeatsInfo> Orders { get; set; }
	}
	public class SeatsInfo
	{
		public int SeatNum { get; set; }
		public string QrCodeFile { get; set; }//Iformfile
	}
}
