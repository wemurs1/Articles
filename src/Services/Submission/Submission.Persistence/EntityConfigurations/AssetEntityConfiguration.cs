using Articles.Abstractions.Enums;
using Blocks.Core.Constraints;
using Blocks.EntityFramework;
using Blocks.EntityFramework.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfigurations;

internal class AssetEntityConfiguration : EntityConfiguration<Asset>
{
    public override void Configure(EntityTypeBuilder<Asset> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Type).HasEnumConversion<AssetType>();
        builder.ComplexProperty(
            o => o.Name, builder =>
            {
                builder.Property(n => n.Value)
                    .HasColumnName(builder.Metadata.PropertyInfo!.Name)
                    .HasMaxLength(MaxLength.C64).IsRequired();
            }
        );
        builder.ComplexProperty(e => e.File, fileBuilder =>
        {
            new FileEntityConfiguration().Configure(fileBuilder);
        });
    }
}
