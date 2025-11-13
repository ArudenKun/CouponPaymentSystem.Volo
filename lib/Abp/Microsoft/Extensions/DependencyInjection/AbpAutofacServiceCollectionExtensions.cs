using Abp;
using Autofac;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpAutofacServiceCollectionExtensions
{
    public static ContainerBuilder GetContainerBuilder(this IServiceCollection services)
    {
        Check.NotNull(services, nameof(services));

        var builder = services.GetObjectOrNull<ContainerBuilder>();
        if (builder == null)
        {
            throw new AbpException($"Could not find ContainerBuilder");
        }

        return builder;
    }

    public static IServiceProvider BuildAutofacServiceProvider(
        this IServiceCollection services,
        Action<ContainerBuilder>? builderAction = null
    )
    {
        return services.BuildServiceProviderFromFactory(builderAction);
    }

    // public static AbpAutofacServiceProviderFactory AddAutofacServiceProviderFactory(
    //     this IServiceCollection services
    // )
    // {
    //     return services.AddAutofacServiceProviderFactory(new ContainerBuilder());
    // }
    //
    // public static AbpAutofacServiceProviderFactory AddAutofacServiceProviderFactory(
    //     this IServiceCollection services,
    //     ContainerBuilder containerBuilder
    // )
    // {
    //     var factory = new AbpAutofacServiceProviderFactory(containerBuilder);
    //
    //     services.AddObjectAccessor(containerBuilder);
    //     services.AddSingleton((IServiceProviderFactory<ContainerBuilder>)factory);
    //
    //     return factory;
    // }
}
