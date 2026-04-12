using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configurations;

public class InterestConfiguration : IEntityTypeConfiguration<Interest>
{
    public void Configure(EntityTypeBuilder<Interest> builder)
    {
        builder.HasData(new List<Interest>()
        {
            new Interest
            {
                Id = new Guid("D284052F-B675-4EFB-83B7-4674D571A988"),
                Name = "Football"
            },
            new Interest
            {
                Id = new Guid("35322244-D9BE-4A99-9E53-3FAA1A70D288"),
                Name = "Sports"
            },
            new Interest
            {
                Id = new Guid("EE2F1F22-D27F-4BFD-A90A-403EEE64F911"),
                Name = "Chess"
            },
            new Interest
            {
                Id = new Guid("194214EA-06AD-4094-8211-DA560C056CD6"),
                Name = "Travelling"
            },
            new Interest
            {
                Id = new Guid("6CF4A9A0-ACD5-46D8-8B2A-BF5985FB5A54"),
                Name = "Coding"
            },
            new Interest
            {
                Id = new Guid("A0CD2CD1-AAC4-4A6A-B307-90A85F454D80"),
                Name = "Cooking"
            }
        });
    }
}