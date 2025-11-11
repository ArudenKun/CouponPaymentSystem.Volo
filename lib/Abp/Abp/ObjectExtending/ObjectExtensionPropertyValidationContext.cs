using System.ComponentModel.DataAnnotations;
using Abp.Data;

namespace Abp.ObjectExtending;

public class ObjectExtensionPropertyValidationContext
{
    /// <summary>
    /// Related property extension information.
    /// </summary>
    public ObjectExtensionPropertyInfo ExtensionPropertyInfo { get; }

    /// <summary>
    /// Reference to the validating object.
    /// </summary>
    public IHasExtraProperties ValidatingObject { get; }

    /// <summary>
    /// Add validation errors to this list.
    /// </summary>
    public List<ValidationResult> ValidationErrors { get; }

    /// <summary>
    /// Validation context comes from the <see cref="IValidatableObject.Validate"/> method.
    /// </summary>
    public ValidationContext ValidationContext { get; }

    /// <summary>
    /// The value of the validating property of the <see cref="ValidatingObject"/>.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Can be used to resolve services from the dependency injection container.
    /// This can be null when SetProperty method is used on the object.
    /// </summary>
    public IServiceProvider ServiceProvider => ValidationContext;

    public ObjectExtensionPropertyValidationContext(
        ObjectExtensionPropertyInfo objectExtensionPropertyInfo,
        IHasExtraProperties validatingObject,
        List<ValidationResult> validationErrors,
        ValidationContext validationContext,
        object? value
    )
    {
        ExtensionPropertyInfo = Check.NotNull(
            objectExtensionPropertyInfo,
            nameof(objectExtensionPropertyInfo)
        );
        ValidatingObject = Check.NotNull(validatingObject, nameof(validatingObject));
        ValidationErrors = Check.NotNull(validationErrors, nameof(validationErrors));
        ValidationContext = Check.NotNull(validationContext, nameof(validationContext));
        Value = value;
    }
}
