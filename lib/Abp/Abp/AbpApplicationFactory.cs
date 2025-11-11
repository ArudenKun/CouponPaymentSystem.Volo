using Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Abp;

public static class AbpApplicationFactory
{
    public static async Task<IAbpApplicationWithInternalServiceProvider> CreateAsync<TStartupModule>(
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IAbpModule
    {
        var app = Create(
            typeof(TStartupModule),
            options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            }
        );
        await app.ConfigureServicesAsync();
        return app;
    }

    public static async Task<IAbpApplicationWithInternalServiceProvider> CreateAsync(
        Type startupModuleType,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
    {
        var app = new AbpApplicationWithInternalServiceProvider(
            startupModuleType,
            options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            }
        );
        await app.ConfigureServicesAsync();
        return app;
    }

    public static async Task<IAbpApplicationWithExternalServiceProvider> CreateAsync<TStartupModule>(
        IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IAbpModule
    {
        var app = Create(
            typeof(TStartupModule),
            services,
            options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            }
        );
        await app.ConfigureServicesAsync();
        return app;
    }

    public static async Task<IAbpApplicationWithExternalServiceProvider> CreateAsync(
        Type startupModuleType,
        IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
    {
        var app = new AbpApplicationWithExternalServiceProvider(
            startupModuleType,
            services,
            options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            }
        );
        await app.ConfigureServicesAsync();
        return app;
    }

    public static IAbpApplicationWithInternalServiceProvider Create<TStartupModule>(
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IAbpModule
    {
        return Create(typeof(TStartupModule), optionsAction);
    }

    public static IAbpApplicationWithInternalServiceProvider Create(
        Type startupModuleType,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
    {
        return new AbpApplicationWithInternalServiceProvider(startupModuleType, optionsAction);
    }

    public static IAbpApplicationWithExternalServiceProvider Create<TStartupModule>(
        IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IAbpModule
    {
        return Create(typeof(TStartupModule), services, optionsAction);
    }

    public static IAbpApplicationWithExternalServiceProvider Create(
        Type startupModuleType,
        IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
    {
        return new AbpApplicationWithExternalServiceProvider(
            startupModuleType,
            services,
            optionsAction
        );
    }
}
