using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Model.Models;

namespace Wasla.DataAccess.ModelsConfig
{
	internal class PublicStationConfig : IEntityTypeConfiguration<PublicStation>
	{
		public void Configure(EntityTypeBuilder<PublicStation> builder)
		{
			builder.HasKey(s => s.StationId);
			
		}
	}
}
