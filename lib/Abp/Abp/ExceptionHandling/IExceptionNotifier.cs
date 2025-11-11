namespace Abp.ExceptionHandling;

public interface IExceptionNotifier
{
    Task NotifyAsync(ExceptionNotificationContext context);
}
