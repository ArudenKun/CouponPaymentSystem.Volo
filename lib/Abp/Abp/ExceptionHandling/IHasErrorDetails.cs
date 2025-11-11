namespace Abp.ExceptionHandling;

public interface IHasErrorDetails
{
    string? Details { get; }
}
