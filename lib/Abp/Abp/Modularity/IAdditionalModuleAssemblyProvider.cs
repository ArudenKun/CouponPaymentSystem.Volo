using System.Reflection;

namespace Abp.Modularity;

public interface IAdditionalModuleAssemblyProvider
{
    Assembly[] GetAssemblies();
}
