using System.Reflection;

namespace Abp.PlugIns;

public class PlugInSourceList : List<IPlugInSource>
{
    public List<Assembly> GetAllAssemblies()
    {
        return this.SelectMany(pluginSource => pluginSource.GetAssemblies()).Distinct().ToList();
    }

    public List<Type> GetAllModules()
    {
        return this.SelectMany(pluginSource => pluginSource.GetModulesWithAllDependencies())
            .Distinct()
            .ToList();
    }
}
