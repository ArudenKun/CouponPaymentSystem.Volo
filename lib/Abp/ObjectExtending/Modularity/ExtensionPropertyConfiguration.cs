using Abp.Localization;
using Abp.Reflection;
using JetBrains.Annotations;

namespace Abp.ObjectExtending.Modularity;

public class ExtensionPropertyConfiguration : IBasicObjectExtensionPropertyInfo
{
    public EntityExtensionConfiguration EntityExtensionConfiguration { get; }

    public string Name { get; }

    public Type Type { get; }

    public List<Attribute> Attributes { get; }

    public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

    public ILocalizableString? DisplayName { get; set; }

    public Dictionary<string, object> Configuration { get; }

    /// <summary>
    /// Single point to enable/disable this property for the clients (UI and API).
    /// If this is false, the configuration made in the <see cref="UI"/> and the <see cref="Api"/>
    /// properties are not used.
    /// Default: true.
    /// </summary>
    public bool IsAvailableToClients { get; set; } = true;

    public ExtensionPropertyEntityConfiguration Entity { get; }

    public ExtensionPropertyUiConfiguration UI { get; }

    public ExtensionPropertyApiConfiguration Api { get; }

    public ExtensionPropertyPolicyConfiguration Policy { get; }

    /// <summary>
    /// Uses as the default value if <see cref="DefaultValueFactory"/> was not set.
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// Used with the first priority to create the default value for the property.
    /// Uses to the <see cref="DefaultValue"/> if this was not set.
    /// </summary>
    public Func<object>? DefaultValueFactory { get; set; }

    public ExtensionPropertyConfiguration(
        EntityExtensionConfiguration entityExtensionConfiguration,
        Type type,
        string name
    )
    {
        EntityExtensionConfiguration = Check.NotNull(
            entityExtensionConfiguration,
            nameof(entityExtensionConfiguration)
        );
        Type = Check.NotNull(type, nameof(type));
        Name = Check.NotNull(name, nameof(name));

        Configuration = new Dictionary<string, object>();
        Attributes = new List<Attribute>();
        Validators = new List<Action<ObjectExtensionPropertyValidationContext>>();

        Entity = new ExtensionPropertyEntityConfiguration();
        UI = new ExtensionPropertyUiConfiguration();
        Api = new ExtensionPropertyApiConfiguration();
        Policy = new ExtensionPropertyPolicyConfiguration();

        Attributes.AddRange(ExtensionPropertyHelper.GetDefaultAttributes(Type));
        DefaultValue = TypeHelper.GetDefaultValue(Type);
    }

    public object? GetDefaultValue()
    {
        return ExtensionPropertyHelper.GetDefaultValue(Type, DefaultValueFactory, DefaultValue);
    }
}
