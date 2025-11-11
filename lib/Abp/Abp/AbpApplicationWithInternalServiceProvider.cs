using Microsoft.Extensions.DependencyInjection;

namespace Abp;

internal class AbpApplicationWithInternalServiceProvider
    : AbpApplicationBase,
        IAbpApplicationWithInternalServiceProvider
{
    public IServiceScope? ServiceScope { get; private set; }

    public AbpApplicationWithInternalServiceProvider(
        Type startupModuleType,
        Action<AbpApplicationCreationOptions>? optionsAction
    )
        : this(startupModuleType, new ServiceCollection(), optionsAction) { }

    private AbpApplicationWithInternalServiceProvider(
        Type startupModuleType,
        IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction
    )
        : base(startupModuleType, services, optionsAction)
    {
        Services.AddSingleton<IAbpApplicationWithInternalServiceProvider>(this);
    }

    public IServiceProvider CreateServiceProvider()
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (ServiceProvider != null)
        {
            return ServiceProvider;
        }

        ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
        SetServiceProvider(ServiceScope.ServiceProvider);

        return ServiceProvider!;
    }

    public async Task InitializeAsync()
    {
        CreateServiceProvider();
        await InitializeModulesAsync();
    }

    public void Initialize()
    {
        CreateServiceProvider();
        InitializeModules();
    }

    public override void Dispose()
    {
        base.Dispose();
        ServiceScope?.Dispose();
    }
}
