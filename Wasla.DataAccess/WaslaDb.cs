using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.DataAccess.ModelsConfig;
using Wasla.Model.Models;

namespace Wasla.DataAccess
{
	public class WaslaDb:IdentityDbContext<User>
	{
        public WaslaDb(DbContextOptions options):base(options)
        {
            
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			#region User Configuration
			modelBuilder.Entity<User>().ToTable("users","Account");
			modelBuilder.Entity<Customer>().ToTable("Customers","Account");
			modelBuilder.Entity<Driver>().ToTable("Drivers", "Account");
			modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Account");
			modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Account");
			modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Account");
			modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Account");
			modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Account");
			modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Account");
			new UserFollowConfiguration().Configure(modelBuilder.Entity<UserFollow>());
			#endregion
			new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());
			new VehicleConfiguraiton().Configure(modelBuilder.Entity<Vehicle>());
			new TripConfiguration().Configure(modelBuilder.Entity<Trip>());
			new PackageConfiguration().Configure(modelBuilder.Entity<Package>());
			new ReservationConfiguration().Configure(modelBuilder.Entity<Reservation>());
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//base.OnConfiguring(optionsBuilder);
		} 
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Driver> Drivers { get; set; }
		public virtual DbSet<Vehicle> Vehicles { get; set; }
		public virtual DbSet<Advertisment> Advertisments { get; set;}
		public virtual DbSet<Organization> Organizations { get; set; }
		public virtual DbSet<Package> Packages { get; set; }
		public virtual DbSet<Reservation> Reservations { get; set; }
		public virtual DbSet<Trip> Trips { get; set; }
	}
}
