using Auth.Domain.Users;
using Blocks.Core.Constraints;
using Blocks.EntityFramework;
using Blocks.EntityFramework.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Persistence.EntityConfigurations;

internal class UserEntityConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Gender).IsRequired().HasEnumConversion();
        builder.OwnsOne(
            e => e.Honourific, builder =>
            {
                builder.Property(e => e.Value)
                    .HasMaxLength(MaxLength.C32)
                    .HasColumnName(nameof(User.Honourific));
                builder.WithOwner();
            }
        );
        builder.OwnsOne(
            e => e.ProfessionalProfile, builder =>
            {
                builder.Property(e => e.Position).HasMaxLength(MaxLength.C32).HasColumnNameSameAsProperty();
                builder.Property(e => e.CompanyName).HasMaxLength(MaxLength.C32).HasColumnNameSameAsProperty();
                builder.Property(e => e.Affiliation).HasMaxLength(MaxLength.C32).HasColumnNameSameAsProperty();
                builder.WithOwner();
            }
        );
        builder.Property(e => e.PictureUrl).HasMaxLength(MaxLength.C2048);

        builder.HasMany(e => e.UserRoles).WithOne().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
