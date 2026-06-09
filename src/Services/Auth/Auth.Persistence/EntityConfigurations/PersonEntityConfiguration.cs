using Auth.Domain.Persons;
using Auth.Domain.Persons.ValueObjects;
using Blocks.Core.Constraints;
using Blocks.EntityFramework;
using Blocks.EntityFramework.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Persistence.EntityConfigurations;

internal class PersonEntityConfiguration : EntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Gender).IsRequired().HasEnumConversion();
        builder.OwnsOne(e => e.Email, b =>
        {
            b.Property(n => n.Value)
                .HasColumnName(nameof(Person.Email))
                .HasMaxLength(MaxLength.C64);
            b.Property(e => e.NormalisedEmail)
                .HasColumnName(nameof(EmailAddress.NormalisedEmail))
                .HasMaxLength(MaxLength.C64);
            b.HasIndex(e => e.NormalisedEmail).IsUnique();
        });
        builder.OwnsOne(
            e => e.Honourific, b =>
            {
                b.Property(e => e.Value)
                    .HasMaxLength(MaxLength.C32)
                    .HasColumnName(nameof(Person.Honourific));
                b.WithOwner();
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
    }
}