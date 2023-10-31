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
<<<<<<< HEAD:Wasla.DataAccess/ModelsConfig/VehicleConfig.cs
	internal class VehicleConfig : IEntityTypeConfiguration<Model.Models.Vehicle>
=======
	internal class VehicleConfiguraiton : IEntityTypeConfiguration<Vehicle>
>>>>>>> origin/Esraa/feature/Auth:Wasla.DataAccess/ModelsConfig/VehicleConfiguraiton.cs
	{
		public void Configure(EntityTypeBuilder<Vehicle> builder)
		{
           // builder.ToTable("Vehicle");
            builder.HasOne(d => d.Orgainzation)
				.WithOne().HasForeignKey<Vehicle>(i => i.OrganizationId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
