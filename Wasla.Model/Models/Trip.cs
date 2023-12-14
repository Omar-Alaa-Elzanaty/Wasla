

namespace Wasla.Model.Models
{
	public class Trip
	{
		public int Id { get; set; }
		public string DriverId { get; set; }
        //public virtual Driver Driver { get; set; }
        public  Driver Driver { get; set; }

        public string OrganizationId { get; set; }
		public  Organization Organization { get; set; }
		public float Price { get; set; }
		public int VehicleId { get; set; }
		public  Vehicle Vehicle { get; set; }
		public TimeSpan Duration { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public int AvailableSets { get; set; }
		public float AvailablePackageSpace { get; set; }
		public  ICollection<Reservation> Reservations { get; set; }
		public  ICollection<Package> Packages { get; set; }
		public Trip() { }
        public Trip(DateTime launchingTime,DateTime arrivingTime)
        {
			if(launchingTime > arrivingTime)
			{
				var temprory = launchingTime;
				launchingTime = arrivingTime;
				arrivingTime= temprory;
			}
			this.Duration = arrivingTime.Subtract(launchingTime);

			if(this.Vehicle is not null)
			{
				AvailablePackageSpace = this.Vehicle.PackageCapcity;
				AvailableSets = this.Vehicle.Capcity;
			}
		}
    }
}
