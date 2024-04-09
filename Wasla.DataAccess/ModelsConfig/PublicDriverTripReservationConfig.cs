using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Model.Models;

namespace Wasla.DataAccess.ModelsConfig
{
    internal class PublicDriverTripReservationConfig : IEntityTypeConfiguration<PublicDriverTripReservation>
    {
        public void Configure(EntityTypeBuilder<PublicDriverTripReservation> builder)
        {
            builder.HasOne(x => x.PublicDriverTrip)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
