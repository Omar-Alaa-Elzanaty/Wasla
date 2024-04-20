namespace Wasla.Model.Dtos
{
    public class TripTimeTableLocationDto
    {
        public TripTimeTableStationDto Start { get; set; }
        public TripTimeTableStationDto End { get; set; }
    }
    public class TripTimeTableStationDto
    {
        public int StationId { get; set; }
        public string Name { get; set; }
        public string Langtitude { get; set; }
        public string Latitude { get; set; }
    }
}
