using Abp.Modules;

namespace Abp.PlugIns;

public static class PlugInSourceExtensions
{
    public static List<Type> GetModulesWithAllDependencies(this IPlugInSource plugInSource)
    {
        return plugInSource
            .GetModules()
            .SelectMany(AbpModule.FindDependedModuleTypesRecursivelyIncludingGivenModule)
            .Distinct()
            .ToList();
    }
}
