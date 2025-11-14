using System.Collections.Immutable;
using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Sources;
using Microsoft.Extensions.Logging;

namespace Abp.Localization;

internal class LocalizationManager : ILocalizationManager
{
    private readonly ILocalizationConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, ILocalizationSource> _sources;
    private readonly ILogger<LocalizationManager> _logger;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LocalizationManager(
        ILocalizationConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<LocalizationManager> logger
    )
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _sources = new Dictionary<string, ILocalizationSource>();
    }

    public void Initialize()
    {
        InitializeSources();
    }

    private void InitializeSources()
    {
        if (!_configuration.IsEnabled)
        {
            _logger.LogDebug("Localization disabled.");
            return;
        }

        _logger.LogDebug("Initializing {0} localization sources.", _configuration.Sources.Count);
        foreach (var source in _configuration.Sources)
        {
            if (_sources.ContainsKey(source.Name))
            {
                throw new AbpException(
                    "There are more than one source with name: "
                        + source.Name
                        + "! Source name must be unique!"
                );
            }

            _sources[source.Name] = source;
            source.Initialize(_configuration, _serviceProvider);

            //Extending dictionaries
            if (source is IDictionaryBasedLocalizationSource dictionaryBasedSource)
            {
                var extensions = _configuration
                    .Sources.Extensions.Where(e => e.SourceName == source.Name)
                    .ToList();
                foreach (var extension in extensions)
                {
                    extension.DictionaryProvider.Initialize(dictionaryBasedSource.Name);
                    foreach (
                        var extensionDictionary in extension.DictionaryProvider.Dictionaries.Values
                    )
                    {
                        dictionaryBasedSource.Extend(extensionDictionary);
                    }
                }
            }

            _logger.LogDebug("Initialized localization source: {Name}", source.Name);
        }
    }

    /// <summary>
    /// Gets a localization source with name.
    /// </summary>
    /// <param name="name">Unique name of the localization source</param>
    /// <returns>The localization source</returns>
    public ILocalizationSource GetSource(string name)
    {
        if (!_configuration.IsEnabled)
        {
            return NullLocalizationSource.Instance;
        }

        if (name == null)
        {
            throw new ArgumentNullException("name");
        }

        ILocalizationSource source;
        if (!_sources.TryGetValue(name, out source))
        {
            throw new AbpException("Can not find a source with name: " + name);
        }

        return source;
    }

    /// <summary>
    /// Gets all registered localization sources.
    /// </summary>
    /// <returns>List of sources</returns>
    public IReadOnlyList<ILocalizationSource> GetAllSources()
    {
        return _sources.Values.ToImmutableList();
    }
}
