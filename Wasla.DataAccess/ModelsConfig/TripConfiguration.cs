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
	public class TripConfiguration : IEntityTypeConfiguration<Trip>
	{
		public void Configure(EntityTypeBuilder<Trip> builder)
		{
			builder.HasOne(t=>t.Organization)
				.WithMany(i=>i.TripList)
				.HasForeignKey(t=>t.OrganizationId)
				.OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(t=>t.vehicle)
				.WithOne(v=>v.Trip)
				.HasForeignKey<Trip>(t=>t.VehicleId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
