using Abp.DependencyInjection;

namespace Abp;

public class ApplicationInitializationContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; set; }

    public ApplicationInitializationContext(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        ServiceProvider = serviceProvider;
    }
}
