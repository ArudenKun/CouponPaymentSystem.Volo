using System.Reflection;
using Stashbox;

namespace Abp.Dependency.Installers;

internal class AssemblyInstaller : IInstaller
{
    private readonly Assembly _assembly;
    private readonly InstallerFactory _factory;

    public AssemblyInstaller(Assembly assembly)
    {
        _assembly = assembly;
        _factory = new InstallerFactory();
    }

    public void Install(IStashboxContainer container)
    {
        var installerTypes = _factory
            .Select(FilterInstallerTypes(_assembly.GetAvailableTypes()))
            .ToArray();
        if (!installerTypes.Any())
            return;

        foreach (var installerType in installerTypes)
        {
            var installer = _factory.CreateInstance(installerType);
            installer.Install(container);
        }
    }

    private static IEnumerable<Type> FilterInstallerTypes(IEnumerable<Type> types) =>
        types.Where(t =>
            t.GetTypeInfo().IsClass
            && !t.GetTypeInfo().IsAbstract
            && !t.GetTypeInfo().IsGenericTypeDefinition
            && t.Is<IInstaller>()
        );

    private sealed class InstallerFactory
    {
        /// <summary>
        ///   Performs custom instantiation of given <param name = "installerType" />
        /// </summary>
        /// <remarks>
        ///   Default implementation uses public parameterless constructor to create the instance.
        /// </remarks>
        public IInstaller CreateInstance(Type installerType) =>
            (IInstaller)Activator.CreateInstance(installerType);

        /// <summary>
        ///   Performs custom filtering/ordering of given set of types.
        /// </summary>
        /// <param name = "installerTypes">Set of concrete class types implementing <see cref = "IInstaller" /> interface.</param>
        /// <returns>Transformed <paramref name = "installerTypes" />.</returns>
        /// <remarks>
        ///   Default implementation simply returns types passed into it.
        /// </remarks>
        public IEnumerable<Type> Select(IEnumerable<Type> installerTypes)
        {
            return installerTypes;
        }
    }
}

file static class ReflectionExtensions
{
    public static bool Is<TType>(this Type type) => typeof(TType).IsAssignableFrom(type);

    public static Type[] GetAvailableTypes(this Assembly assembly, bool includeNonExported = false)
    {
        try
        {
            if (includeNonExported)
            {
                return assembly.GetTypes();
            }

            return assembly.GetExportedTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(t => t != null).ToArray();
        }
    }
}
