using Abp.DependencyInjection;

namespace Abp.Modularity;

public interface IModuleLifecycleContributor : ITransientDependency
{
    Task InitializeAsync(ApplicationInitializationContext context, IAbpModule module);

    void Initialize(ApplicationInitializationContext context, IAbpModule module);

    Task ShutdownAsync(ApplicationShutdownContext context, IAbpModule module);

    void Shutdown(ApplicationShutdownContext context, IAbpModule module);
}
