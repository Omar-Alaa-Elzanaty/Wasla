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
	public class DriverConfiguration : IEntityTypeConfiguration<Driver>
	{
		public void Configure(EntityTypeBuilder<Driver> builder)
		{
          //  builder.ToTable("Driver");

            builder.HasOne(d=>d.Orgainzation)
				.WithOne().HasForeignKey<Driver>(i=>i.OrganizationId).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
