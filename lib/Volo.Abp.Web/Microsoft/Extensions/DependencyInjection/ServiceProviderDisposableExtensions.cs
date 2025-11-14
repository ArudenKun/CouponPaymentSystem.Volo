using Volo.Abp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderDisposableExtensions
{
    extension(IServiceProvider serviceProvider)
    {
        public IDisposableServiceWrapper<T> GetRequiredServiceAsDisposable<T>()
        {
            return new DisposableServiceWrapper<T>(serviceProvider, typeof(T));
        }

        public IDisposableServiceWrapper<T> GetRequiredServiceAsDisposable<T>(Type serviceType)
            where T : notnull
        {
            return new DisposableServiceWrapper<T>(serviceProvider, serviceType);
        }
    }
}
