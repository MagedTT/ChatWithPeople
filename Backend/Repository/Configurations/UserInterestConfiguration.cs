using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
{
    public void Configure(EntityTypeBuilder<UserInterest> builder)
    {
        builder.HasKey(x => new { x.UserId, x.InterestId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserInterests)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(x => x.Interest)
            .WithMany(x => x.UserInterests)
            .HasForeignKey(x => x.InterestId);
    }
}