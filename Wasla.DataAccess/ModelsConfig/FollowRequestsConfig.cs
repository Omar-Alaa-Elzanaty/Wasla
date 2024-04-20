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
	internal class FollowRequestsConfig : IEntityTypeConfiguration<FollowRequests>
	{
		public void Configure(EntityTypeBuilder<FollowRequests> builder)
		{
			builder.HasKey(uf => new {uf.SenderId,uf.FollowerId});
			builder.HasOne(uf => uf.Sender).WithMany().HasForeignKey(u => u.SenderId);
			builder.HasOne(uf => uf.Follower).WithMany().HasForeignKey(uf => uf.FollowerId)
				.OnDelete(deleteBehavior:DeleteBehavior.NoAction);
		}
	}
}
