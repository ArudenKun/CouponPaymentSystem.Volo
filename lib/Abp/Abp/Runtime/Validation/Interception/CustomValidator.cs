using System.ComponentModel.DataAnnotations;

namespace Abp.Runtime.Validation.Interception;

public class CustomValidator : IMethodParameterValidator
{
    private readonly IServiceProvider _serviceProvider;

    public CustomValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IReadOnlyList<ValidationResult> Validate(object validatingObject)
    {
        var validationErrors = new List<ValidationResult>();

        if (validatingObject is ICustomValidate customValidateObject)
        {
            var context = new CustomValidationContext(validationErrors, _serviceProvider);
            customValidateObject.AddValidationErrors(context);
        }

        return validationErrors;
    }
}
