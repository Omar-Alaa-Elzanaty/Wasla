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
    internal class LineConfig : IEntityTypeConfiguration<Line>
    {
        void IEntityTypeConfiguration<Line>.Configure(EntityTypeBuilder<Line> builder)
        {
            builder.HasOne(x=>x.Start)
                   .WithMany()
                   .HasForeignKey(x=>x.StartId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
