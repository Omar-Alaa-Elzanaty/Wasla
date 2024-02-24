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
	internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.HasOne(e => e.Organization)
				.WithMany(o => o.Employees)
				.HasForeignKey(e => e.OrgId)
				.OnDelete(deleteBehavior:DeleteBehavior.NoAction);

		}
	}
}
