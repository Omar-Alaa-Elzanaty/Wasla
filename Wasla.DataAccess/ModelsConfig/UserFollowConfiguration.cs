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
	internal class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
	{
		public void Configure(EntityTypeBuilder<UserFollow> builder)
		{
			builder.HasKey(uf => new {uf.UserId,uf.FollowerId});
			builder.HasOne(uf => uf.User).WithOne().HasForeignKey<UserFollow>(u => u.UserId);
			builder.HasOne(uf => uf.Follower).WithOne().HasForeignKey<UserFollow>(uf => uf.FollowerId);
		}
	}
}
