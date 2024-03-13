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
	internal class UserFollowConfig : IEntityTypeConfiguration<UserFollow>
	{
		public void Configure(EntityTypeBuilder<UserFollow> builder)
		{
			builder.HasKey(uf => new {uf.CustomerId,uf.FollowerId});
			builder.HasOne(uf => uf.Customer).WithMany().HasForeignKey(u => u.CustomerId);
			builder.HasOne(uf => uf.Follower).WithMany().HasForeignKey(uf => uf.FollowerId)
				.OnDelete(deleteBehavior:DeleteBehavior.NoAction);
		}
	}
}
