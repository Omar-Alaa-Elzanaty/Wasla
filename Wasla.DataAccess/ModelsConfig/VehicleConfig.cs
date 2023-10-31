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
            builder.HasOne(d => d.Orgainzation)
				.WithOne().HasForeignKey<Vehicle>(i => i.OrganizationId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
