using System.Collections.Concurrent;
using System.Collections.Immutable;
using Abp.Collections.Extensions;
using JetBrains.Annotations;

namespace Abp.ObjectExtending;

public class ObjectExtensionManager
{
    public static ObjectExtensionManager Instance { get; protected set; } =
        new ObjectExtensionManager();

    public ConcurrentDictionary<object, object> Configuration { get; }

    protected ConcurrentDictionary<Type, ObjectExtensionInfo> ObjectsExtensions { get; }

    protected internal ObjectExtensionManager()
    {
        ObjectsExtensions = new ConcurrentDictionary<Type, ObjectExtensionInfo>();
        Configuration = new ConcurrentDictionary<object, object>();
    }

    public virtual ObjectExtensionManager AddOrUpdate<TObject>(
        Action<ObjectExtensionInfo>? configureAction = null
    )
    {
        return AddOrUpdate(typeof(TObject), configureAction);
    }

    public virtual ObjectExtensionManager AddOrUpdate(
        Type[] types,
        Action<ObjectExtensionInfo>? configureAction = null
    )
    {
        Check.NotNull(types, nameof(types));

        foreach (var type in types)
        {
            AddOrUpdate(type, configureAction);
        }

        return this;
    }

    public virtual ObjectExtensionManager AddOrUpdate(
        Type type,
        Action<ObjectExtensionInfo>? configureAction = null
    )
    {
        var extensionInfo = ObjectsExtensions.GetOrAdd(type, _ => new ObjectExtensionInfo(type));

        configureAction?.Invoke(extensionInfo);

        return this;
    }

    public virtual ObjectExtensionInfo? GetOrNull<TObject>()
    {
        return GetOrNull(typeof(TObject));
    }

    public virtual ObjectExtensionInfo? GetOrNull(Type type)
    {
        return ObjectsExtensions.GetOrDefault(type);
    }

    public virtual ImmutableList<ObjectExtensionInfo> GetExtendedObjects()
    {
        return ObjectsExtensions.Values.ToImmutableList();
    }
}
