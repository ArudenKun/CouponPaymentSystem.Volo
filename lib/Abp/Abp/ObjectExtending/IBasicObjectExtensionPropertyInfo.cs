namespace Abp.ObjectExtending;

public interface IBasicObjectExtensionPropertyInfo
{
    public string Name { get; }

    public Type Type { get; }

    public List<Attribute> Attributes { get; }

    public List<Action<ObjectExtensionPropertyValidationContext>> Validators { get; }

    public string? DisplayName { get; }

    /// <summary>
    /// Uses as the default value if <see cref="DefaultValueFactory"/> was not set.
    /// </summary>
    public object? DefaultValue { get; set; }

    /// <summary>
    /// Used with the first priority to create the default value for the property.
    /// Uses to the <see cref="DefaultValue"/> if this was not set.
    /// </summary>
    public Func<object>? DefaultValueFactory { get; set; }
}
