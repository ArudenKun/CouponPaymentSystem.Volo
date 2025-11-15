using System.Collections.Concurrent;
using System.Collections.Immutable;
using Abp.Collections.Extensions;
using JetBrains.Annotations;

namespace Abp.ObjectExtending;

public class ObjectExtensionInfo
{
    public Type Type { get; }

    protected ConcurrentDictionary<string, ObjectExtensionPropertyInfo> Properties { get; }

    public ConcurrentDictionary<object, object> Configuration { get; }

    public List<Action<ObjectExtensionValidationContext>> Validators { get; }

    public ObjectExtensionInfo(Type type)
    {
        Type = Check.NotNull(type, nameof(type));
        Properties = new ConcurrentDictionary<string, ObjectExtensionPropertyInfo>();
        Configuration = new ConcurrentDictionary<object, object>();
        Validators = new List<Action<ObjectExtensionValidationContext>>();
    }

    public virtual bool HasProperty(string propertyName)
    {
        return Properties.ContainsKey(propertyName);
    }

    public virtual ObjectExtensionInfo AddOrUpdateProperty<TProperty>(
        string propertyName,
        Action<ObjectExtensionPropertyInfo>? configureAction = null
    )
    {
        return AddOrUpdateProperty(typeof(TProperty), propertyName, configureAction);
    }

    public virtual ObjectExtensionInfo AddOrUpdateProperty(
        Type propertyType,
        string propertyName,
        Action<ObjectExtensionPropertyInfo>? configureAction = null
    )
    {
        Check.NotNull(propertyType, nameof(propertyType));
        Check.NotNull(propertyName, nameof(propertyName));

        var propertyInfo = Properties.GetOrAdd(
            propertyName,
            _ => new ObjectExtensionPropertyInfo(this, propertyType, propertyName)
        );

        configureAction?.Invoke(propertyInfo);

        return this;
    }

    public virtual ImmutableList<ObjectExtensionPropertyInfo> GetProperties()
    {
        return Properties.OrderBy(t => t.Value.UI.Order).Select(t => t.Value).ToImmutableList();
    }

    public virtual ObjectExtensionPropertyInfo? GetPropertyOrNull(string propertyName)
    {
        Check.NotNullOrEmpty(propertyName, nameof(propertyName));

        return Properties.GetOrDefault(propertyName);
    }
}
