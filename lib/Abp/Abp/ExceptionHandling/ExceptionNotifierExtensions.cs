using Microsoft.Extensions.Logging;

namespace Abp.ExceptionHandling;

public static class ExceptionNotifierExtensions
{
    public static Task NotifyAsync(
        this IExceptionNotifier exceptionNotifier,
        Exception exception,
        LogLevel? logLevel = null,
        bool handled = true
    )
    {
        Check.NotNull(exceptionNotifier, nameof(exceptionNotifier));

        return exceptionNotifier.NotifyAsync(
            new ExceptionNotificationContext(exception, logLevel, handled)
        );
    }
}
