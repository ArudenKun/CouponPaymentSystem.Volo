using System.ComponentModel.DataAnnotations;

namespace Abp.Validation;

public interface IHasValidationErrors
{
    IList<ValidationResult> ValidationErrors { get; }
}
