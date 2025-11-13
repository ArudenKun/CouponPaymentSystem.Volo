using Abp.DependencyInjection;

namespace Abp.Authorization;

internal class PermissionDependencyContext : IPermissionDependencyContext, ITransientDependency
{
    public UserIdentifier? User { get; set; }

    public IServiceProvider ServiceProvider { get; }

    public IPermissionChecker PermissionChecker { get; }

    public PermissionDependencyContext(
        IServiceProvider serviceProvider,
        IPermissionChecker permissionChecker
    )
    {
        ServiceProvider = serviceProvider;
        PermissionChecker = permissionChecker;
    }
}
