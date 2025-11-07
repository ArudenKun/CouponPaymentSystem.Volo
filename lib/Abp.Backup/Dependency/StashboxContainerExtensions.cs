using System.Reflection;
using Abp.Dependency.Installers;
using Stashbox;

namespace Abp.Dependency;

public static class StashboxContainerExtensions
{
    public static void Install(this IStashboxContainer container, IInstaller installer) =>
        installer.Install(container);

    public static void Install(this IStashboxContainer container, Assembly assembly)
    {
        var installer = new AssemblyInstaller(assembly);
        installer.Install(container);
    }

    public static void Install(this IStashboxContainer container, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
            container.Install(assembly);
    }
}
