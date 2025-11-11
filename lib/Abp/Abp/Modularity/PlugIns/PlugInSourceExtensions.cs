using Microsoft.Extensions.Logging;

namespace Abp.Modularity.PlugIns;

public static class PlugInSourceExtensions
{
    public static Type[] GetModulesWithAllDependencies(
        this IPlugInSource plugInSource,
        ILogger logger
    )
    {
        Check.NotNull(plugInSource, nameof(plugInSource));

        return plugInSource
            .GetModules()
            .SelectMany(type => AbpModuleHelper.FindAllModuleTypes(type, logger))
            .Distinct()
            .ToArray();
    }
}
