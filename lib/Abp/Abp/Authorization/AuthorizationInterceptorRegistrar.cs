using System.Reflection;
using Abp.Application.Features;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Authorization;

/// <summary>
/// This class is used to register interceptors on the Application Layer.
/// </summary>
internal static class AuthorizationInterceptorRegistrar
{
    public static void Initialize(IServiceCollection services)
    {
        services.OnRegistered(context =>
        {
            if (ShouldIntercept(context.ImplementationType))
            {
                context.Interceptors.TryAdd<AuthorizationInterceptor>();
            }
        });
    }

    private static bool ShouldIntercept(Type type)
    {
        if (SelfOrMethodsDefinesAttribute<AbpAuthorizeAttribute>(type))
        {
            return true;
        }

        if (SelfOrMethodsDefinesAttribute<RequiresFeatureAttribute>(type))
        {
            return true;
        }

        return false;
    }

    private static bool SelfOrMethodsDefinesAttribute<TAttribute>(Type type)
    {
        if (type.GetTypeInfo().IsDefined(typeof(TAttribute), true))
        {
            return true;
        }

        return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(m => m.IsDefined(typeof(TAttribute), true));
    }
}
