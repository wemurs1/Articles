namespace Review.Persistence.EntityConfigurations;

public class ReviewerEntityConfiguration : IEntityTypeConfiguration<Reviewer>
{
    public void Configure(EntityTypeBuilder<Reviewer> builder)
    {
        builder.HasBaseType<Person>();

        builder.HasMany(r=>r.Specialisations).WithOne(j=>j.Reviewer);
    }
}
