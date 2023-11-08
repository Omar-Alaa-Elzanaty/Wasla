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
		public string DriverId { get; set; }
		public virtual Driver Driver { get; set; }
		public string OrganizationId { get; set; }
		public virtual Organization Organization { get; set; }
		public float Price { get; set; }
		public int VehicleId { get; set; }
		public virtual Vehicle vehicle { get; set; }
		public TimeSpan Duration { get; set; }
		public string From { get; set; }
		public string To { get; set; }
		public int AvailableSets { get; set; }
		public float AvailablePackageSpace { get; set; }
		public virtual ICollection<Reservation> Reservations { get; set; }
		public virtual ICollection<Package> Packages { get; set; }
		public Trip() { }
        public Trip(TimeSpan launchingTime,TimeSpan arrivingTime)
        {
			if(launchingTime > arrivingTime)
			{
				var temporary = launchingTime;
				launchingTime = arrivingTime;
				arrivingTime= temporary;
			}
			this.Duration = arrivingTime.Subtract(launchingTime);

			if(this.vehicle is not null)
			{
				AvailablePackageSpace = this.vehicle.PackageCapcity;
				AvailableSets = this.vehicle.Capcity;
			}
		}
    }
}
