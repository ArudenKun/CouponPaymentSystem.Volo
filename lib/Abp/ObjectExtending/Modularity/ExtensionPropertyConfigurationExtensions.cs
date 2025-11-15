using Abp.Localization;

namespace Abp.ObjectExtending.Modularity;

public static class ExtensionPropertyConfigurationExtensions
{
    public static string? GetLocalizationSourceNameOrNull(
        this ExtensionPropertyConfiguration property
    )
    {
        if (property.DisplayName is LocalizableString localizableString)
        {
            return localizableString.SourceName;
        }

        return null;
    }
}
