using Abp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderDisposeExtensions
{
    extension(IServiceProvider serviceProvider)
    {
        public IDisposableDependencyServiceWrapper<TService> GetRequiredServiceAsDisposable<TService>(
            Type serviceType
        )
            where TService : notnull
        {
            return new DisposableDependencyServiceWrapper<TService>(
                serviceProvider.GetRequiredService<IServiceScopeFactory>()
            );
        }

        public IDisposableDependencyServiceWrapper<T> GetRequiredServiceAsDisposable<T>()
            where T : notnull
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            return new DisposableDependencyServiceWrapper<T>(scopeFactory);
        }

        public IDisposableDependencyServiceWrapper GetRequiredServiceAsDisposable(Type serviceType)
        {
            var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
            return new DisposableDependencyServiceWrapper(scopeFactory, serviceType);
        }
    }
}
