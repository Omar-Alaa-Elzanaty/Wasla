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
<<<<<<< HEAD:Wasla.DataAccess/ModelsConfig/TripConfig.cs
	internal class TripConfig : IEntityTypeConfiguration<Trip>
=======
	public class TripConfiguration : IEntityTypeConfiguration<Trip>
>>>>>>> origin/Esraa/feature/Auth:Wasla.DataAccess/ModelsConfig/TripConfiguration.cs
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
