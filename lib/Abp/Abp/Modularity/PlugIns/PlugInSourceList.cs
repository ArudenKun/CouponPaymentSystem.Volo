using Microsoft.Extensions.Logging;

namespace Abp.Modularity.PlugIns;

public class PlugInSourceList : List<IPlugInSource>
{
    internal Type[] GetAllModules(ILogger logger)
    {
        return this.SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies(logger))
            .Distinct()
            .ToArray();
    }
}
