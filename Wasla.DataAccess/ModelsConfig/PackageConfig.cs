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
<<<<<<< HEAD:Wasla.DataAccess/ModelsConfig/PackageConfig.cs
	internal class PackageConfig : IEntityTypeConfiguration<Package>
=======
	public class PackageConfiguration : IEntityTypeConfiguration<Package>
>>>>>>> origin/Esraa/feature/Auth:Wasla.DataAccess/ModelsConfig/PackageConfiguration.cs
	{
		public void Configure(EntityTypeBuilder<Package> builder)
		{
			builder.HasOne(i=>i.Trip)
				.WithMany(t=>t.Packages)
				.HasForeignKey(p=>p.TripId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
