using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(x => x.FriendshipsSent)
            .WithOne(x => x.User1)
            .HasForeignKey(x => x.User1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.FriendshipsReceived)
            .WithOne(x => x.User2)
            .HasForeignKey(x => x.User2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.SentFriendRequests)
            .WithOne(x => x.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.ReceivedFriendRequests)
            .WithOne(x => x.Receiver)
            .HasForeignKey(x => x.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.MessagesSent)
            .WithOne(x => x.Sender)
            .HasForeignKey(x => x.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.GroupsCreated)
            .WithOne(x => x.CreatedBy)
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Notifications)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}