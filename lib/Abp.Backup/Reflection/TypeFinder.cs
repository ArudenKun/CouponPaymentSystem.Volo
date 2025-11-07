using System.Reflection;
using Abp.Collections.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Abp.Reflection;

public class TypeFinder : ITypeFinder
{
    public ILogger Logger { get; set; }

    private readonly IAssemblyFinder _assemblyFinder;
    private readonly object _syncObj = new();
    private Type[] _types;

    public TypeFinder(IAssemblyFinder assemblyFinder)
    {
        _assemblyFinder = assemblyFinder;
        Logger = NullLogger.Instance;
    }

    public Type[] Find(Func<Type, bool> predicate)
    {
        return GetAllTypes().Where(predicate).ToArray();
    }

    public Type[] FindAll()
    {
        return GetAllTypes().ToArray();
    }

    private Type[] GetAllTypes()
    {
        if (_types == null)
        {
            lock (_syncObj)
            {
                if (_types == null)
                {
                    _types = CreateTypeList().ToArray();
                }
            }
        }

        return _types;
    }

    private List<Type> CreateTypeList()
    {
        var allTypes = new List<Type>();

        var assemblies = _assemblyFinder.GetAllAssemblies().Distinct();

        foreach (var assembly in assemblies)
        {
            try
            {
                Type[] typesInThisAssembly;

                try
                {
                    typesInThisAssembly = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    typesInThisAssembly = ex.Types;
                }

                if (typesInThisAssembly.IsNullOrEmpty())
                {
                    continue;
                }

                allTypes.AddRange(typesInThisAssembly.Where(type => type != null));
            }
            catch (Exception ex)
            {
                Logger.LogWarning(
                    ex,
                    "Failed to find types in assembly {Assembly}",
                    assembly.FullName
                );
            }
        }

        return allTypes;
    }
}
