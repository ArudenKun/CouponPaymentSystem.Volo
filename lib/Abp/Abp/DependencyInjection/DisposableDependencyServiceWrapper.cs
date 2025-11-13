using Microsoft.Extensions.DependencyInjection;

namespace Abp.DependencyInjection;

internal class DisposableDependencyServiceWrapper : IDisposableDependencyServiceWrapper
{
    private readonly IServiceScope _serviceScope;

    public DisposableDependencyServiceWrapper(
        IServiceScopeFactory serviceScopeFactory,
        Type serviceType
    )
    {
        _serviceScope = serviceScopeFactory.CreateScope();
        Service = _serviceScope.ServiceProvider.GetRequiredService(serviceType);
    }

    public object Service { get; }

    public void Dispose() => _serviceScope.Dispose();
}

internal class DisposableDependencyServiceWrapper<T> : IDisposableDependencyServiceWrapper<T>
    where T : notnull
{
    private readonly IServiceScope _serviceScope;

    public DisposableDependencyServiceWrapper(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScope = serviceScopeFactory.CreateScope();
        Service = _serviceScope.ServiceProvider.GetRequiredService<T>();
    }

    public T Service { get; }

    public void Dispose() => _serviceScope.Dispose();
}
