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
	internal class TripConfig : IEntityTypeConfiguration<Trip>
	{
		public void Configure(EntityTypeBuilder<Trip> builder)
		{
			builder.HasOne(t=>t.Organization)
				.WithMany(i=>i.TripList)
				.HasForeignKey(t=>t.OrganizationId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.Line)
				.WithMany().HasForeignKey(x=>x.LineId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
