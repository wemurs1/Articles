using Blocks.Core.Constraints;

namespace Review.Persistence.EntityConfigurations;

public class PersonEntityConfiguration : EntityConfiguration<Person>
{
    protected override bool HasGeneratedId => false;

    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.UserId).IsUnique();

        // builder.HasIndex(p => new { p.EmailAddress.Value, p.TypeDescriminator }).IsUnique();
        // using raw SQL here because at this moment we cannot use a value object to create a composite index
        builder.HasAnnotation(
            "RawSql:Index",
            "CREATE UNIQUE INDEX IX_Person_TypeDescriminator ON Person (Email, TypeDescriminator)"
        );

        builder.HasDiscriminator(e => e.TypeDescriminator)
            .HasValue<Person>(nameof(Person))
            .HasValue<Author>(nameof(Author))
            .HasValue<Reviewer>(nameof(Reviewer))
            .HasValue<Editor>(nameof(Editor));

        builder.Property(e => e.FirstName).HasMaxLength(MaxLength.C64).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(MaxLength.C64).IsRequired();
        builder.Property(e => e.Honourific).HasMaxLength(MaxLength.C32);
        builder.Property(e => e.Affiliation).HasMaxLength(512).IsRequired()
            .HasComment("Institution or organisation they are associated with when they do their ouw research");
        builder.Property(e => e.UserId).IsRequired(false);

        builder.ComplexProperty(o => o.Email, builder =>
        {
            builder.Property(n => n.Value)
                .HasColumnName(builder.Metadata.PropertyInfo?.Name)
                .HasMaxLength(MaxLength.C64);
        });
    }
}
