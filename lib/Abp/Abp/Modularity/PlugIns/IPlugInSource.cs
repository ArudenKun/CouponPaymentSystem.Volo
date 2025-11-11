namespace Abp.Modularity.PlugIns;

public interface IPlugInSource
{
    Type[] GetModules();
}
