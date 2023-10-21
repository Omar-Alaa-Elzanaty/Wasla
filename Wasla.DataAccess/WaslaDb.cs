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
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//base.OnConfiguring(optionsBuilder);
		}
		public virtual DbSet<Customer> Customers { get; set; }
		public virtual DbSet<Driver> Drivers { get; set; }
	}
}
