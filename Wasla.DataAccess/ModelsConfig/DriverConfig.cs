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
<<<<<<< HEAD:Wasla.DataAccess/ModelsConfig/DriverConfig.cs
	internal class DriverConfig : IEntityTypeConfiguration<Driver>
	{
		public void Configure(EntityTypeBuilder<Driver> builder)
		{
			builder.HasOne(d=>d.Orgainzation)
				.WithOne().HasForeignKey<Driver>(i=>i.OrganizationId).OnDelete(DeleteBehavior.NoAction);
=======
	public class DriverConfiguration : IEntityTypeConfiguration<Driver>
	{
		public void Configure(EntityTypeBuilder<Driver> builder)
		{
          //  builder.ToTable("Driver");

            builder.HasOne(d=>d.Orgainzation)
				.WithOne().HasForeignKey<Driver>(i=>i.OrganizationId).OnDelete(DeleteBehavior.Cascade);
>>>>>>> origin/Esraa/feature/Auth:Wasla.DataAccess/ModelsConfig/DriverConfiguration.cs
		}
	}
}
