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
	internal class VehicleConfiguraiton : IEntityTypeConfiguration<Model.Models.Vehicle>
	{
		public void Configure(EntityTypeBuilder<Model.Models.Vehicle> builder)
		{
			builder.HasOne(d => d.Orgainzation)
				.WithOne().HasForeignKey<Model.Models.Vehicle>(i => i.OrganizationId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
