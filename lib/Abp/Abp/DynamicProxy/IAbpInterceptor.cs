namespace Abp.DynamicProxy;

public interface IAbpInterceptor
{
    Task InterceptAsync(IAbpMethodInvocation invocation);
}
