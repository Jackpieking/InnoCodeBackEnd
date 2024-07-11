using System;

namespace InnoCode.Application.Share.BackgroundJob;

public interface IRecurringJob
{
    public TimeSpan Frequency { get; set; }
}
