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
	internal class OrganizationRateConfig : IEntityTypeConfiguration<OrganizationRate>
	{
		public void Configure(EntityTypeBuilder<OrganizationRate> builder)
		{
			builder.HasKey(r => new {r.OrgId,r.CustomerId});
			
			builder.HasOne<Organization>()
				.WithMany(o=>o.Rates)
				.HasForeignKey(r=>r.OrgId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
