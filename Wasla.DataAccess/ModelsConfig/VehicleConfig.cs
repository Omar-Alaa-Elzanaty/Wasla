using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasla.Model.Models;

namespace Wasla.DataAccess.ModelsConfig
{
	internal class VehicleConfig : IEntityTypeConfiguration<Vehicle>
	{
		public void Configure(EntityTypeBuilder<Vehicle> builder)
		{
           // builder.ToTable("Vehicle");
    //        builder.HasOne<Organization>()
				//.WithMany().HasForeignKey(i => i.OrganizationId).OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(v=>v.Trips)
				.WithOne(t=>t.Vehicle)
				.HasForeignKey(t=>t.VehicleId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.PublicDriver)
				.WithOne(x => x.Vehicle)
				.HasForeignKey<Vehicle>(x => x.PublicDriverId);
		}
	}
}
