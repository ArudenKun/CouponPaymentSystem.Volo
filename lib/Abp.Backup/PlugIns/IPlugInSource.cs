using System.Reflection;

namespace Abp.PlugIns;

public interface IPlugInSource
{
    List<Assembly> GetAssemblies();

    List<Type> GetModules();
}
