using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class GroupJoinRequestConfiguration : IEntityTypeConfiguration<GroupJoinRequest>
{
    public void Configure(EntityTypeBuilder<GroupJoinRequest> builder)
    {
        builder.HasOne(x => x.Group)
            .WithMany(x => x.JoinRequests)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.GroupJoinRequests)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}