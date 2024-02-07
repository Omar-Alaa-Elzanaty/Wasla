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
    internal class publicDriverConfig : IEntityTypeConfiguration<PublicDriverRate>
    {
        public void Configure(EntityTypeBuilder<PublicDriverRate> builder)
        {
            builder.HasKey(dr => new { dr.DriverId, dr.CustomerId });
            builder.HasOne(dr => dr.Driver).WithMany(d => d.Rates).HasForeignKey(dr => dr.DriverId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
