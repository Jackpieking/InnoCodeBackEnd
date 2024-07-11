using InnoCode.Domain.Entities;
using InnoCode.PostgresSql.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InnoCode.PostgresSql.Data.EntityConfigurations;

internal sealed class AppExceptionLoggingEntityConfiguration
    : IEntityTypeConfiguration<AppExceptionLoggingEntity>
{
    public void Configure(EntityTypeBuilder<AppExceptionLoggingEntity> builder)
    {
        builder.ToTable(
            name: MetaData.Table.TableName,
            schema: MetaData.Table.TableSchema,
            buildAction: table => table.HasComment(comment: "Contain app exception loggings.")
        );

        builder
            .HasKey(keyExpression: entity => entity.Id)
            .HasName(name: MetaData.Property.Id.ColumnName);

        builder
            .Property(propertyExpression: entity => entity.ErrorMessage)
            .HasColumnName(name: MetaData.Property.ErrorMessage.ColumnName)
            .HasColumnType(typeName: CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(required: true);

        builder
            .Property(propertyExpression: entity => entity.ErrorStackTrace)
            .HasColumnName(name: MetaData.Property.ErrorStackTrace.ColumnName)
            .HasColumnType(typeName: CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(required: true);

        builder
            .Property(propertyExpression: entity => entity.CreatedAt)
            .HasColumnName(name: MetaData.Property.CreatedAt.ColumnName)
            .HasColumnType(typeName: CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(required: true);

        builder
            .Property(propertyExpression: entity => entity.Data)
            .HasColumnName(name: MetaData.Property.Data.ColumnName)
            .HasColumnType(typeName: CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(required: true);
    }

    internal static class MetaData
    {
        internal static class Table
        {
            internal static readonly string TableName = "app_logging_exception";

            internal static readonly string TableSchema = CommonConstant.DatabaseSchemaName.MAIN;
        }

        internal static class Property
        {
            internal static class Id
            {
                internal static readonly string ColumnName = nameof(AppExceptionLoggingEntity.Id);
            }

            internal static class ErrorMessage
            {
                internal static readonly string ColumnName = nameof(
                    AppExceptionLoggingEntity.ErrorMessage
                );
            }

            internal static class ErrorStackTrace
            {
                internal static readonly string ColumnName = nameof(
                    AppExceptionLoggingEntity.ErrorStackTrace
                );
            }

            internal static class CreatedAt
            {
                internal static readonly string ColumnName = nameof(
                    AppExceptionLoggingEntity.CreatedAt
                );
            }

            internal static class Data
            {
                internal static readonly string ColumnName = nameof(AppExceptionLoggingEntity.Data);
            }
        }
    }
}
