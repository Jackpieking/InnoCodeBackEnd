using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class DictionaryEntityConfiguration : IEntityTypeConfiguration<DictionaryEntity>
{
    public void Configure(EntityTypeBuilder<DictionaryEntity> builder)
    {
        builder.ToTable(
            MetaData.Table.TableName,
            MetaData.Table.TableSchema,
            table => table.HasComment("Contain dictionaries")
        );

        builder
            .Property(entity => entity.Key)
            .HasColumnName(MetaData.Property.Key.ColumnName)
            .HasColumnType(CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(true);

        builder
            .Property(entity => entity.Value)
            .HasColumnName(MetaData.Property.Key.ColumnName)
            .HasColumnType(CommonConstant.DatabaseNativeType.JSONB)
            .IsRequired(true);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "dictionary";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.MAIN;
        }

        internal static class Property
        {
            internal static class Key
            {
                internal static readonly string ColumnName = nameof(DictionaryEntity.Key);
            }

            internal static class Value
            {
                internal static readonly string ColumnName = nameof(DictionaryEntity.Value);
            }
        }
    }
}
