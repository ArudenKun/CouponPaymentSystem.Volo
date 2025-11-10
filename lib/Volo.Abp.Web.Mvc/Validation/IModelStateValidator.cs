using System.Web.Mvc;
using Volo.Abp.Validation;

namespace Volo.Abp.Web.Mvc.Validation;

public interface IModelStateValidator
{
    void Validate(ModelStateDictionary modelState);

    void AddErrors(IAbpValidationResult validationResult, ModelStateDictionary modelState);
}
