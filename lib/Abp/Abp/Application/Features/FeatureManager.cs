using System.Collections.Immutable;
using Abp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Application.Features;

/// <summary>
/// Implements <see cref="IFeatureManager"/>.
/// </summary>
internal class FeatureManager : FeatureDefinitionContextBase, IFeatureManager, ISingletonDependency
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IFeatureConfiguration _featureConfiguration;

    /// <summary>
    /// Creates a new <see cref="FeatureManager"/> object
    /// </summary>
    /// <param name="serviceScopeFactory">IOC Manager</param>
    /// <param name="featureConfiguration">Feature configuration</param>
    public FeatureManager(
        IServiceScopeFactory serviceScopeFactory,
        IFeatureConfiguration featureConfiguration
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _featureConfiguration = featureConfiguration;
    }

    /// <summary>
    /// Initializes this <see cref="FeatureManager"/>
    /// </summary>
    public void Initialize()
    {
        foreach (var providerType in _featureConfiguration.Providers)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = (FeatureProvider)scope.ServiceProvider.GetRequiredService(providerType);
            provider.SetFeatures(this);
        }

        Features.AddAllFeatures();
    }

    /// <summary>
    /// Gets a feature by its given name
    /// </summary>
    /// <param name="name">Name of the feature</param>
    public Feature Get(string name)
    {
        var feature = GetOrNull(name);
        if (feature == null)
        {
            throw new AbpException("There is no feature with name: " + name);
        }

        return feature;
    }

    /// <summary>
    /// Gets all the features
    /// </summary>
    public IReadOnlyList<Feature> GetAll()
    {
        return Features.Values.ToImmutableList();
    }
}
