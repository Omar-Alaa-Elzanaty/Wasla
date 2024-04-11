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
    internal class publicDriverConfig : IEntityTypeConfiguration<PublicDriver>
    {
        public void Configure(EntityTypeBuilder<PublicDriver> builder)
        {
            builder.HasOne(x => x.Vehicle)
                .WithOne(x => x.PublicDriver)
                .HasForeignKey<PublicDriver>(x => x.VehicleId);
        }
    }
}
