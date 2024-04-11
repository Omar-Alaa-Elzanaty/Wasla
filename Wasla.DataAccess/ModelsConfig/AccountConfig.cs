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
    internal class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasQueryFilter(a => string.IsNullOrEmpty(a.Email));
            builder.HasQueryFilter(a => string.IsNullOrEmpty(a.NormalizedEmail));
            builder.HasQueryFilter(a => string.IsNullOrEmpty(a.PhoneNumber));
        }
    }
}
