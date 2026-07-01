namespace ArticleHub.Persistence.EntityConfigurations;

public class PersonEntityConfiguration : EntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.UserId).IsRequired(false);
        builder.Property(e => e.FirstName).HasMaxLength(MaxLength.C64).IsRequired();
        builder.Property(e => e.LastName).HasMaxLength(MaxLength.C64).IsRequired();
        builder.Property(e => e.Honourific).HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Email).HasMaxLength(MaxLength.C256).IsRequired();
    }
}
