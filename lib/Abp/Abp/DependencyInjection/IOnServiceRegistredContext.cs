using Abp.Collections;
using Abp.DynamicProxy;

namespace Abp.DependencyInjection;

public interface IOnServiceRegistredContext
{
    ITypeList<IAbpInterceptor> Interceptors { get; }

    Type ImplementationType { get; }

    object? ServiceKey { get; }
}
