using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Abp.Domain.Uow;

/// <summary>
/// This class is used to register interceptor for needed classes for Unit Of Work mechanism.
/// </summary>
internal static class UnitOfWorkRegistrar
{
    /// <summary>
    /// Initializes the registerer.
    /// </summary>
    /// <param name="services">IOC manager</param>
    public static void Initialize(IServiceCollection services)
    {
        services.OnRegistered(ctx =>
        {
            if (ShouldIntercept(services, ctx.ImplementationType.GetTypeInfo()))
            {
                ctx.Interceptors.TryAdd<UnitOfWorkInterceptor>();
            }
        });
    }

    private static bool ShouldIntercept(IServiceCollection services, TypeInfo implementationType)
    {
        if (IsUnitOfWorkType(implementationType) || AnyMethodHasUnitOfWork(implementationType))
        {
            return true;
        }

        var uowOptions = services.GetRequiredService<IOptions<UnitOfWorkDefaultOptions>>();
        return uowOptions.Value.IsConventionalUowClass(implementationType.AsType());
    }

    private static bool IsUnitOfWorkType(TypeInfo implementationType)
    {
        return UnitOfWorkHelper.HasUnitOfWorkAttribute(implementationType);
    }

    private static bool AnyMethodHasUnitOfWork(TypeInfo implementationType)
    {
        return implementationType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(UnitOfWorkHelper.HasUnitOfWorkAttribute);
    }
}
