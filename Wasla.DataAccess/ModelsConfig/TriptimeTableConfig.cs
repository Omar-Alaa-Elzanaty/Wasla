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
    internal class TriptimeTableConfig : IEntityTypeConfiguration<TripTimeTable>
    {
        public void Configure(EntityTypeBuilder<TripTimeTable> builder)
        {
            builder.HasMany(x => x.Reservations)
                .WithOne(x=>x.TripTimeTable).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
