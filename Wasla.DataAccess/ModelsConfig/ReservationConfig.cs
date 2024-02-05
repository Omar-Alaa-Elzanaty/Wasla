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
	internal class ReservationConfig : IEntityTypeConfiguration<Reservation>
	{
		public void Configure(EntityTypeBuilder<Reservation> builder)
		{
			//builder.HasOne(r=>r.Customer)
			//	.WithMany(c=>c.Reservations)
			//	.HasForeignKey(i=>i.Customer).OnDelete(DeleteBehavior.NoAction);

		}
	}
}
