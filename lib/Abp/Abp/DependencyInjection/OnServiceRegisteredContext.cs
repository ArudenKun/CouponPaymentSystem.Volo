using Abp.Collections;
using Abp.DynamicProxy;

namespace Abp.DependencyInjection;

public class OnServiceRegisteredContext : IOnServiceRegistredContext
{
    public virtual ITypeList<IAbpInterceptor> Interceptors { get; }

    public virtual Type ServiceType { get; }

    public virtual Type ImplementationType { get; }

    public virtual object? ServiceKey { get; }

    public OnServiceRegisteredContext(
        Type serviceType,
        Type implementationType,
        object? serviceKey = null
    )
    {
        ServiceType = Check.NotNull(serviceType, nameof(serviceType));
        ImplementationType = Check.NotNull(implementationType, nameof(implementationType));
        ServiceKey = serviceKey;

        Interceptors = new TypeList<IAbpInterceptor>();
    }
}
