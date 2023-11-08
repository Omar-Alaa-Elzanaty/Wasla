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
	internal class VehicleRateConfig : IEntityTypeConfiguration<VehicleRate>
	{
		public void Configure(EntityTypeBuilder<VehicleRate> builder)
		{
			builder.HasKey(vr => new {vr.CustomerId, vr.VehicleId});
			builder.HasIndex(vr => vr.CustomerId).IsUnique();
			builder.HasOne(vr => vr.Vehicle).WithMany(v=>v.Rate).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
