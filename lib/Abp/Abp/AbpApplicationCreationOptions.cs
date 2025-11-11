using Abp.Modularity.PlugIns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abp;

public class AbpApplicationCreationOptions
{
    public IServiceCollection Services { get; }

    public PlugInSourceList PlugInSources { get; }

    /// <summary>
    /// The options in this property only take effect when IConfiguration not registered.
    /// </summary>
    public AbpConfigurationBuilderOptions Configuration { get; }

    public bool SkipConfigureServices { get; set; }

    public string? ApplicationName { get; set; }

    public string? Environment { get; set; }

    public AbpApplicationCreationOptions(IServiceCollection services)
    {
        Services = Check.NotNull(services, nameof(services));
        PlugInSources = new PlugInSourceList();
        Configuration = new AbpConfigurationBuilderOptions();
    }
}
