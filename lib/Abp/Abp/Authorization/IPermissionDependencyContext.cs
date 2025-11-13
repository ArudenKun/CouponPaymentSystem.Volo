using Abp.Application.Features;

namespace Abp.Authorization;

/// <summary>
/// Permission dependency context.
/// </summary>
public interface IPermissionDependencyContext
{
    /// <summary>
    /// The user which requires permission. Can be null if no user.
    /// </summary>
    UserIdentifier? User { get; }

    /// <summary>
    /// Gets the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <value>
    /// The ioc resolver.
    /// </value>
    IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the <see cref="IFeatureChecker"/>.
    /// </summary>
    /// <value>
    /// The feature checker.
    /// </value>
    IPermissionChecker PermissionChecker { get; }
}
