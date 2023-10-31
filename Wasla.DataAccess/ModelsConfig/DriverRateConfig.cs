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
	internal class DriverRateConfig : IEntityTypeConfiguration<DriverRate>
	{
		public void Configure(EntityTypeBuilder<DriverRate> builder)
		{
			builder.HasKey(dr => new { dr.DriverId, dr.CustomerId });
			builder.HasIndex(dr=>dr.CustomerId).IsUnique();
			builder.HasOne(dr=>dr.Driver).WithMany(d=>d.Rates).HasForeignKey(dr=>dr.DriverId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}
