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
            MetaData.Table.TableName,
            MetaData.Table.TableSchema,
            table => table.HasComment("Contain app exception loggings")
        );

        builder.HasKey(entity => entity.Id);

        builder.Property(entity => entity.Id).HasColumnName(MetaData.Property.Id.ColumnName);

        builder
            .Property(entity => entity.ErrorMessage)
            .HasColumnName(MetaData.Property.ErrorMessage.ColumnName)
            .HasColumnType(CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(true);

        builder
            .Property(entity => entity.ErrorStackTrace)
            .HasColumnName(MetaData.Property.ErrorStackTrace.ColumnName)
            .HasColumnType(CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(true);

        builder
            .Property(entity => entity.CreatedAt)
            .HasColumnName(MetaData.Property.CreatedAt.ColumnName)
            .HasColumnType(CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(true);

        builder
            .Property(entity => entity.Data)
            .HasColumnName(MetaData.Property.Data.ColumnName)
            .HasColumnType(CommonConstant.DatabaseNativeType.TEXT)
            .IsRequired(true);
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
