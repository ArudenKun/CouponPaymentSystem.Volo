using Abp.ObjectExtending;

namespace Abp.Mapperly;

[AttributeUsage(AttributeTargets.Class)]
public class MapExtraPropertiesAttribute : Attribute
{
    public MappingPropertyDefinitionChecks DefinitionChecks { get; set; } =
        MappingPropertyDefinitionChecks.Null;

    public string[]? IgnoredProperties { get; set; }

    public bool MapToRegularProperties { get; set; }
}
