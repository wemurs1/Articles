namespace Submission.Persistence.EntityConfigurations;

internal class AssetTypeDefinitionEntityConfiguration : IEntityTypeConfiguration<AssetTypeDefinition>
{
    public void Configure(EntityTypeBuilder<AssetTypeDefinition> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Name).HasEnumConversion().HasMaxLength(MaxLength.C64).IsRequired().HasColumnOrder(1);
        builder.Property(e => e.MaxFileSizeInMB).HasDefaultValue(5);
        builder.Property(e => e.DefaultFileExtension).HasMaxLength(MaxLength.C8).HasDefaultValue("pdf").IsRequired();
        builder.ComplexProperty(e => e.AllowedFileExtensions, builder =>
        {
            var converter = BuilderExtensions.BuildJsonListConvertor<string>();
            builder.Property(e => e.Extensions)
                .HasConversion(converter)
                .HasColumnName(builder.Metadata.PropertyInfo!.Name)
                .IsRequired();
        });
    }
}
