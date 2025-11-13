using Abp.DependencyInjection;

namespace Abp.Application.Features;

/// <summary>
/// Implementation of <see cref="IFeatureDependencyContext"/>.
/// </summary>
public class FeatureDependencyContext : IFeatureDependencyContext, ITransientDependency
{
    public Guid? TenantId { get; set; }

    /// <inheritdoc/>
    public IServiceProvider ServiceProvider { get; }

    /// <inheritdoc/>
    public IFeatureChecker FeatureChecker { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FeatureDependencyContext"/> class.
    /// </summary>
    public FeatureDependencyContext(
        IServiceProvider serviceProvider,
        IFeatureChecker featureChecker
    )
    {
        ServiceProvider = serviceProvider;
        FeatureChecker = featureChecker;
    }
}
