using System.ComponentModel.DataAnnotations;
using Abp.ObjectExtending.Data;
using JetBrains.Annotations;

namespace Abp.ObjectExtending;

public class ObjectExtensionValidationContext
{
    /// <summary>
    /// Related object extension information.
    /// </summary>
    public ObjectExtensionInfo ObjectExtensionInfo { get; }

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
    /// Can be used to resolve services from the dependency injection container.
    /// </summary>
    public IServiceProvider? ServiceProvider => ValidationContext;

    public ObjectExtensionValidationContext(
        ObjectExtensionInfo objectExtensionInfo,
        IHasExtraProperties validatingObject,
        List<ValidationResult> validationErrors,
        ValidationContext validationContext
    )
    {
        ObjectExtensionInfo = Check.NotNull(objectExtensionInfo, nameof(objectExtensionInfo));
        ValidatingObject = Check.NotNull(validatingObject, nameof(validatingObject));
        ValidationErrors = Check.NotNull(validationErrors, nameof(validationErrors));
        ValidationContext = Check.NotNull(validationContext, nameof(validationContext));
    }
}
