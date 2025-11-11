namespace Abp;

public class ApplicationShutdownContext
{
    public IServiceProvider ServiceProvider { get; }

    public ApplicationShutdownContext(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        ServiceProvider = serviceProvider;
    }
}
