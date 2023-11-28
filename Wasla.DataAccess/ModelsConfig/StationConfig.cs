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
	internal class StationConfig : IEntityTypeConfiguration<Station>
	{
		public void Configure(EntityTypeBuilder<Station> builder)
		{
			builder.HasKey(s => new {s.OrganizationId, s.Name});
			
		}
	}
}
