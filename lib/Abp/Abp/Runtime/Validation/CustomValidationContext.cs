using System.ComponentModel.DataAnnotations;

namespace Abp.Runtime.Validation;

public class CustomValidationContext
{
    /// <summary>
    /// List of validation results (errors). Add validation errors to this list.
    /// </summary>
    public List<ValidationResult> Results { get; }

    /// <summary>
    /// Can be used to resolve dependencies on validation.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    public CustomValidationContext(List<ValidationResult> results, IServiceProvider serviceProvider)
    {
        Results = results;
        ServiceProvider = serviceProvider;
    }
}
