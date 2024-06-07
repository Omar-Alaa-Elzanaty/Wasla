using System.Numerics;

namespace Wasla.Model.Dtos
{
	public class SeatInfo
	{
		public string Name { get; set; }
		public int SeatNum { get; set; }
		public string? QrCodeFile { get; set; }//Iformfile
	}
	public class ReservationDto
	{
		public int TripId { get; set; }
		public bool OnRoad { get; set; }
		public string LocationDescription { get; set; }
        public List<SeatInfo> seats { get; set; }
	}
}
