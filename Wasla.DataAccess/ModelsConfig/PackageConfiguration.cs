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
	internal class PackageConfiguration : IEntityTypeConfiguration<Package>
	{
		public void Configure(EntityTypeBuilder<Package> builder)
		{
			builder.HasOne(i=>i.Trip)
				.WithMany(t=>t.Packages)
				.HasForeignKey(p=>p.TripId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
