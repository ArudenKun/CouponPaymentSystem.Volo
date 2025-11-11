using Abp.Modularity.PlugIns;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Modularity;

public interface IModuleLoader
{
    IAbpModuleDescriptor[] LoadModules(
        IServiceCollection services,
        Type startupModuleType,
        PlugInSourceList plugInSources
    );
}
