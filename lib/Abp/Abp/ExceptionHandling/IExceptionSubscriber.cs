namespace Abp.ExceptionHandling;

public interface IExceptionSubscriber
{
    Task HandleAsync(ExceptionNotificationContext context);
}
