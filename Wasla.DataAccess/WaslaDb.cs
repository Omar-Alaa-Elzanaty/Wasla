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
	public class WaslaDb:IdentityDbContext<Account>
	{
        public WaslaDb(DbContextOptions options):base(options)
        {
            
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
<<<<<<< HEAD
			#region User Configuration
			modelBuilder.Entity<Account>().ToTable("Accounts", "Account");
			modelBuilder.Entity<User>().ToTable("Users","Account");
=======
         //   modelBuilder.ApplyConfigurationsFromAssembly(typeof(DriverConfiguration).Assembly);
            #region User Configuration
            modelBuilder.Entity<User>().ToTable("users","Account");
>>>>>>> origin/Esraa/feature/Auth
			modelBuilder.Entity<Customer>().ToTable("Customers","Account");
			modelBuilder.Entity<Driver>().ToTable("Drivers", "Account");
			modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Account");
			modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Account");
			modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Account");
			modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Account");
			modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Account");
			modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Account");
			new UserFollowConfig().Configure(modelBuilder.Entity<UserFollow>());
			#endregion
			new DriverConfig().Configure(modelBuilder.Entity<Driver>());
			new VehicleConfig().Configure(modelBuilder.Entity<Vehicle>());
			new TripConfig().Configure(modelBuilder.Entity<Trip>());
			new PackageConfig().Configure(modelBuilder.Entity<Package>());
			new ReservationConfig().Configure(modelBuilder.Entity<Reservation>());
			new VehicleRateConfig().Configure(modelBuilder.Entity<VehicleRate>());
			new DriverRateConfig().Configure(modelBuilder.Entity<DriverRate>());
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
		public virtual DbSet<VehicleRate> VehicleRates { get; set; }
		public virtual DbSet<DriverRate> DriverRates { get; set; }
		public virtual DbSet<CustomerTripOrder> CustomerTripOrders { get; set; }
	}
}
