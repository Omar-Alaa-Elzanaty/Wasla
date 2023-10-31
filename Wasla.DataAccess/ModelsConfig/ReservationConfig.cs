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
<<<<<<< HEAD:Wasla.DataAccess/ModelsConfig/ReservationConfig.cs
	internal class ReservationConfig : IEntityTypeConfiguration<Reservation>
=======
	public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
>>>>>>> origin/Esraa/feature/Auth:Wasla.DataAccess/ModelsConfig/ReservationConfiguration.cs
	{
		public void Configure(EntityTypeBuilder<Reservation> builder)
		{
			//builder.HasOne(r=>r.Customer)
			//	.WithMany(c=>c.Reservations)
			//	.HasForeignKey(i=>i.Customer).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(r => r.Trip).WithMany(c => c.Reservations)
				.OnDelete(DeleteBehavior.NoAction);

		}
	}
}
