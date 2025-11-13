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
    extension(IServiceCollection services)
    {
        internal void AddCoreServices()
        {
            services.AddOptions();
            services.AddLogging();
        }

        internal void AddCoreAbpServices(
            IAbpApplication abpApplication,
            AbpApplicationCreationOptions applicationCreationOptions
        )
        {
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

            services.Configure<AbpModuleLifecycleOptions>(options =>
            {
                options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
            });
        }
    }
}
