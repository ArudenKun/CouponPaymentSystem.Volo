using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

internal class DisposableServiceWrapper<T> : IDisposableServiceWrapper<T>
{
    private readonly IServiceScope _serviceScope;
    private bool _isDisposed;

    public DisposableServiceWrapper(IServiceProvider serviceProvider, Type serviceType)
    {
        _serviceScope = serviceProvider.CreateScope();
        Service = (T)_serviceScope.ServiceProvider.GetRequiredService(serviceType);
    }

    public T Service { get; }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        _serviceScope.Dispose();
        _isDisposed = true;
    }
}
