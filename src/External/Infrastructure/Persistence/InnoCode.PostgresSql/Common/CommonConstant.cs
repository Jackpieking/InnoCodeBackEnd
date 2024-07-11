namespace InnoCode.PostgresSql.Common;

internal static class CommonConstant
{
    internal static class DatabaseNativeType
    {
        internal static readonly string TEXT = "TEXT";

        internal static readonly string TIMESTAMPTZ = "TIMESTAMPTZ";

        internal static readonly string JSONB = "JSONB";
    }

    internal static class DatabaseSchemaName
    {
        internal static readonly string MAIN = "main";

        internal static readonly string SLAVE = "slave";

        internal static readonly string DEFAULT = "public";
    }

    internal static class DatabaseCollation
    {
        public static readonly string CASE_INSENSITIVE = "case_insensitive";
    }
}
