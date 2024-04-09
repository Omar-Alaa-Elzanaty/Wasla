using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wasla.Model.Models;

namespace Wasla.DataAccess.ModelsConfig
{
    internal class PublicDriverTripRequestConfig : IEntityTypeConfiguration<PublicDriverTripRequest>
    {
        public void Configure(EntityTypeBuilder<PublicDriverTripRequest> builder)
        {
            builder.HasOne(x => x.PublicDriverTrip)
                .WithMany(x => x.Requests)
                .OnDelete(deleteBehavior: DeleteBehavior.Restrict);
        }
    }
}
