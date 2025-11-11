using Abp;
using Abp.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionApplicationExtensions
{
    public static IAbpApplicationWithExternalServiceProvider AddApplication<TStartupModule>(
        this IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IAbpModule
    {
        return AbpApplicationFactory.Create<TStartupModule>(services, optionsAction);
    }

    public static IAbpApplicationWithExternalServiceProvider AddApplication(
        this IServiceCollection services,
        Type startupModuleType,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
    {
        return AbpApplicationFactory.Create(startupModuleType, services, optionsAction);
    }

    public static async Task<IAbpApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
        this IServiceCollection services,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
        where TStartupModule : IAbpModule
    {
        return await AbpApplicationFactory.CreateAsync<TStartupModule>(services, optionsAction);
    }

    public static async Task<IAbpApplicationWithExternalServiceProvider> AddApplicationAsync(
        this IServiceCollection services,
        Type startupModuleType,
        Action<AbpApplicationCreationOptions>? optionsAction = null
    )
    {
        return await AbpApplicationFactory.CreateAsync(startupModuleType, services, optionsAction);
    }

    public static string? GetApplicationName(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfoAccessor>().ApplicationName;
    }

    public static string GetApplicationInstanceId(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IApplicationInfoAccessor>().InstanceId;
    }

    public static IAbpHostEnvironment GetAbpHostEnvironment(this IServiceCollection services)
    {
        return services.GetSingletonInstance<IAbpHostEnvironment>();
    }
}
