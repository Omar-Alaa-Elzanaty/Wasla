﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wasla.DataAccess.ModelsConfig;
using Wasla.Model.Models;

namespace Wasla.DataAccess
{
	public class WaslaDb:IdentityDbContext<Account>
	{
        public WaslaDb(DbContextOptions options):base(options)
        {
            
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			#region User Configuration
			modelBuilder.Entity<Account>().ToTable("Accounts", "Account");
			modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Account");
			modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Account");
			modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Account");
			modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Account");
			modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Account");
			modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Account");
			modelBuilder.Entity<Account>().HasIndex(x => x.Email).IsUnique(false);
			#endregion

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(DriverConfig).Assembly);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//base.OnConfiguring(optionsBuilder);
		}
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Driver> Drivers { get; set; }
		public virtual DbSet<Vehicle> Vehicles { get; set; }
		public virtual DbSet<Advertisment> Advertisments { get; set;}
		public virtual DbSet<Organization> Organizations { get; set; }
		public virtual DbSet<Package> Packages { get; set; }
		public virtual DbSet<Reservation> Reservations { get; set; }
		public virtual DbSet<Trip> Trips { get; set; }
		public virtual DbSet<VehicleRate> VehicleRates { get; set; }
		public virtual DbSet<DriverRate> DriverRates { get; set; }
		public virtual DbSet<CustomerTripOrder> CustomerTripOrders { get; set; }
		public virtual DbSet<OrganizationRegisterRequest> OrganizationsRegisters { get; set; }
		public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<PublicStation> PublicStations { get; set; }

        public virtual DbSet<Seat> Seats { get; set; }
		public virtual DbSet<TripTimeTable> TripTimeTables { get; set;}
		public virtual DbSet<Line> Lines { get; set; }
		public virtual DbSet<PublicDriver> PublicDrivers { get; set; }
		public virtual DbSet<PublicDriverRate> PublicDriversRates { get; set; }
	}
}
