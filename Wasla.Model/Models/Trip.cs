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
		public int Capacity { get; set; }
		public float AvailablePackageSpace { get; set; }
		public virtual List<Set> RecervedSets {  get; set; }
		public virtual ICollection<Reservation> Reservations { get; set; }
		public virtual ICollection<Package> Packages { get; set; }
		public Trip()
		{
			RecervedSets = new List<Set>();
			Reservations= new List<Reservation>();
			Packages = new List<Package>();
		}
		public Trip(DateTime launchingTime, DateTime arrivingTime)
		{
			if (launchingTime > arrivingTime)
			{
				var temprory = launchingTime;
				launchingTime = arrivingTime;
				arrivingTime = temprory;
			}
			this.Duration = arrivingTime.Subtract(launchingTime);

			if (this.Vehicle is not null)
			{
				AvailablePackageSpace = this.Vehicle.PackageCapcity;
				AvailableSets = this.Vehicle.Capcity;
			}
		}
	public class Set
	{
		public int setNum { get; set; }
		public int TripId { get; set; }
	}
}
