namespace Abp.ExceptionHandling;

public interface IHasHttpStatusCode
{
    int HttpStatusCode { get; }
}
