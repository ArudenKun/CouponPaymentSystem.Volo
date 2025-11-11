using Abp.DynamicProxy;
using Abp.Logging;
using Abp.Modularity;
using Abp.Reflection;
using Abp.SimpleStateChecking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Abp.Internal;

internal static class InternalServiceCollectionExtensions
{
    internal static void AddCoreServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddLogging();
    }

    internal static void AddCoreAbpServices(
        this IServiceCollection services,
        IAbpApplication abpApplication,
        AbpApplicationCreationOptions applicationCreationOptions
    )
    {
        services.AddAutofacServiceProviderFactory();

        var moduleLoader = new ModuleLoader();
        var assemblyFinder = new AssemblyFinder(abpApplication);

        if (!services.IsAdded<IConfiguration>())
        {
            services.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(applicationCreationOptions.Configuration)
            );
        }

        services.TryAddSingleton<IModuleLoader>(moduleLoader);
        services.TryAddSingleton<IAssemblyFinder>(assemblyFinder);
        services.TryAddSingleton<IInitLoggerFactory>(new DefaultInitLoggerFactory());
        var typeFinder = new TypeFinder(services.GetInitLogger<TypeFinder>(), assemblyFinder);
        services.TryAddSingleton<ITypeFinder>(typeFinder);

        services.AddAssemblyOf<IAbpApplication>();

        services.AddTransient(
            typeof(ISimpleStateCheckerManager<>),
            typeof(SimpleStateCheckerManager<>)
        );

        services.AddTransient(typeof(AbpAsyncDeterminationInterceptor<>));

        services.Configure<AbpModuleLifecycleOptions>(options =>
        {
            options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
            options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
        });
    }
}
