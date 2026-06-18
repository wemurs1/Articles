using Articles.Abstractions.Enums;
using Blocks.Core.Constraints;
using Review.Domain.Assets;

namespace Review.Persistence.EntityConfigurations;

public class AssetTypeDefinitionEntityConfiguration : EnumEntityConfiguration<AssetTypeDefinition, AssetType>
{
    public override void Configure(EntityTypeBuilder<AssetTypeDefinition> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.MaxAssetCount).HasDefaultValue(1);

        builder.Property(e => e.DefaultFileExtension).HasDefaultValue("pdf").IsRequired().HasMaxLength(MaxLength.C8);

        builder.ComplexProperty(e => e.AllowedFileExtensions, b =>
        {
            var converter = BuilderExtensions.BuildJsonReadOnlyListConvertor<string>();
            b.Property(e => e.Extensions)
                .HasConversion(converter)
                .HasColumnName(b.Metadata.PropertyInfo!.Name)
                .IsRequired();
        });
    }
}
