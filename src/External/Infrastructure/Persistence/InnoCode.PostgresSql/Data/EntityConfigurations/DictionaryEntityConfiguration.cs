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
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain dictionaries.")
        );

        builder
            .Property(propertyExpression: entity => entity.Key)
            .HasColumnName(name: MetaData.Property.Key.ColumnName)
            .HasColumnType(typeName: CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(required: true);

        builder
            .Property(propertyExpression: entity => entity.Value)
            .HasColumnName(name: MetaData.Property.Key.ColumnName)
            .HasColumnType(typeName: CommonConstant.DatabaseNativeType.JSONB)
            .IsRequired(required: true);
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
