using Microsoft.Extensions.Logging;

namespace Abp.Logging;

public static class HasLogLevelExtensions
{
    public static TException WithLogLevel<TException>(this TException exception, LogLevel logLevel)
        where TException : IHasLogLevel
    {
        Check.NotNull(exception, nameof(exception));

        exception.LogLevel = logLevel;

        return exception;
    }
}
