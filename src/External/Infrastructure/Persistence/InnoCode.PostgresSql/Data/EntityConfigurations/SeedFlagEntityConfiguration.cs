using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class SeedFlagEntityConfiguration : IEntityTypeConfiguration<SeedFlagEntity>
{
    public void Configure(EntityTypeBuilder<SeedFlagEntity> builder)
    {
        builder.ToTable(
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain seed flags.")
        );

        builder
            .HasKey(keyExpression: builder => builder.Id)
            .HasName(name: MetaData.Property.Id.ColumnName);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "seed_flag";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.MAIN;
        }

        internal static class Property
        {
            internal static class Id
            {
                internal static readonly string ColumnName = nameof(SeedFlagEntity.Id);
            }
        }
    }
}
