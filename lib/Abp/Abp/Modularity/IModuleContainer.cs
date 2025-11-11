namespace Abp.Modularity;

public interface IModuleContainer
{
    IReadOnlyList<IAbpModuleDescriptor> Modules { get; }
}
