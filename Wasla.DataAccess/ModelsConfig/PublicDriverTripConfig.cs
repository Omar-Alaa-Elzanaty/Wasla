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
    internal class PublicDriverTripConfig : IEntityTypeConfiguration<PublicDriverTrip>
    {
        public void Configure(EntityTypeBuilder<PublicDriverTrip> builder)
        {
            builder.HasOne(x=>x.StartStation)
                .WithMany()
                .HasForeignKey(x=>x.StartStationId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.EndStation)
                .WithMany()
                .HasForeignKey(x=>x.EndStationId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
