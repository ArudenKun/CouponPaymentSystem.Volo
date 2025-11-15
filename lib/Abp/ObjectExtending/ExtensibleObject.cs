using System.ComponentModel.DataAnnotations;
using Abp.ObjectExtending.Data;
using Abp.Reflection;

namespace Abp.ObjectExtending;

[Serializable]
public class ExtensibleObject : IHasExtraProperties, IValidatableObject
{
    public ExtraPropertyDictionary ExtraProperties { get; protected set; }

    public ExtensibleObject()
        : this(true) { }

    public ExtensibleObject(bool setDefaultsForExtraProperties)
    {
        ExtraProperties = new ExtraPropertyDictionary();

        if (setDefaultsForExtraProperties)
        {
            this.SetDefaultsForExtraProperties(ProxyHelper.UnProxy(this).GetType());
        }
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return ExtensibleObjectValidator.GetValidationErrors(this, validationContext);
    }
}
