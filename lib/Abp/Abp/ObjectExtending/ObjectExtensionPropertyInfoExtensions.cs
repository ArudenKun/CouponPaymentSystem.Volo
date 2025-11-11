using System.ComponentModel.DataAnnotations;

namespace Abp.ObjectExtending;

public static class ObjectExtensionPropertyInfoExtensions
{
    public static ValidationAttribute[] GetValidationAttributes(
        this ObjectExtensionPropertyInfo propertyInfo
    )
    {
        return propertyInfo.Attributes.OfType<ValidationAttribute>().ToArray();
    }
}
