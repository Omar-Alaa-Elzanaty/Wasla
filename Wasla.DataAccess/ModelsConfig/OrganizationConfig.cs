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
	internal class OrganizationConfig : IEntityTypeConfiguration<Organization>
	{
		public void Configure(EntityTypeBuilder<Organization> builder)
		{
			builder.HasOne(o => o.Account)
				.WithOne()
				.HasForeignKey<Organization>(o => o.AccountId);
		}
	}
}
