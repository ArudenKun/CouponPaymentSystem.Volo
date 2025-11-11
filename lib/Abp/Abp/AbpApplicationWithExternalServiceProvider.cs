using Microsoft.Extensions.DependencyInjection;

namespace Abp;

internal class AbpApplicationWithExternalServiceProvider
    : AbpApplicationBase,
        IAbpApplicationWithExternalServiceProvider
{
    public AbpApplicationWithExternalServiceProvider(
        Type startupModuleType,
        IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction
    )
        : base(startupModuleType, services, optionsAction)
    {
        services.AddSingleton<IAbpApplicationWithExternalServiceProvider>(this);
    }

    void IAbpApplicationWithExternalServiceProvider.SetServiceProvider(
        IServiceProvider serviceProvider
    )
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (ServiceProvider != null)
        {
            if (ServiceProvider != serviceProvider)
            {
                throw new AbpException(
                    "Service provider was already set before to another service provider instance."
                );
            }

            return;
        }

        SetServiceProvider(serviceProvider);
    }

    public async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        await InitializeModulesAsync();
    }

    public void Initialize(IServiceProvider serviceProvider)
    {
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        SetServiceProvider(serviceProvider);

        InitializeModules();
    }

    public override void Dispose()
    {
        base.Dispose();

        if (ServiceProvider is IDisposable disposableServiceProvider)
        {
            disposableServiceProvider.Dispose();
        }
    }
}
