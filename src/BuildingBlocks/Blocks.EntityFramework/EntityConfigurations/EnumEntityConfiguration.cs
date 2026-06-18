using Blocks.Core.Constraints;
using Blocks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blocks.EntityFrameworkCore.EntityConfigurations;

public abstract class EnumEntityConfiguration<TEntity, TEnum> : EntityConfiguration<TEntity, TEnum>
    where TEntity : EnumEntity<TEnum>
    where TEnum : struct, Enum
{
    public override void Configure(EntityTypeBuilder<TEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Name).HasEnumConversion().IsRequired().HasMaxLength(MaxLength.C64).HasColumnOrder(1);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(MaxLength.C512).HasColumnOrder(2);
    }
}
