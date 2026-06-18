namespace Submission.Persistence.EntityConfigurations;

public class JournalEntityConfiguration : EntityConfiguration<Journal>
{
    public override void Configure(EntityTypeBuilder<Journal> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Name).HasMaxLength(64).IsRequired();
        builder.Property(e => e.Abbreviation).HasMaxLength(8).IsRequired();
    }
}
