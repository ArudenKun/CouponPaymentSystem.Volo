namespace Abp.ExceptionHandling;

public interface IHasErrorCode
{
    string? Code { get; }
}
