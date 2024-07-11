using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable(
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain users.")
        );
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "user";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.DEFAULT;
        }
    }
}
