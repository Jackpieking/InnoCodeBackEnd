using System;
using InnoCode.Domain.Common;

namespace InnoCode.Domain.Entities;

public sealed class AppExceptionLoggingEntity : IEntity
{
    public Guid Id { get; set; }

    public string ErrorMessage { get; set; }

    public string ErrorStackTrace { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Data { get; set; }

    public static class MetaData
    {
        public static class Property
        {
            public static class ErrorMessage
            {
                public const int MinLength = 2;
            }

            public static class ErrorStackTrace
            {
                public const int MinLength = 2;
            }

            public static class Data
            {
                public const int MinLength = 2;
            }
        }
    }
}
