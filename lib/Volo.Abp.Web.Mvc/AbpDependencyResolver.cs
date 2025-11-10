using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Web.Mvc;

public class AbpDependencyResolver : IDependencyResolver
{
    private readonly IServiceProvider _serviceProvider;

    public AbpDependencyResolver(IServiceProvider serviceProvider) =>
        _serviceProvider = serviceProvider;

    public object GetService(Type serviceType) => _serviceProvider.GetService(serviceType);

    public IEnumerable<object?> GetServices(Type serviceType) =>
        _serviceProvider.GetServices(serviceType);
}
