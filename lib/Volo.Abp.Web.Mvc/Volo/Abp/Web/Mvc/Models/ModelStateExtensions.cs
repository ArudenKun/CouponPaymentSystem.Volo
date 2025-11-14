using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.Localization;
using Volo.Abp.Web.Models;
using Volo.Abp.Web.Resources;

namespace Volo.Abp.Web.Mvc.Models;

public static class ModelStateExtensions
{
    public static AjaxResponse ToAjaxResponse(this ModelStateDictionary modelState)
    {
        if (modelState.IsValid)
        {
            return new AjaxResponse();
        }

        var validationErrors = new List<ValidationErrorInfo>();

        foreach (var state in modelState)
        {
            foreach (var error in state.Value.Errors)
            {
                validationErrors.Add(new ValidationErrorInfo(error.ErrorMessage, state.Key));
            }
        }

        var errorInfo = new ErrorInfo(
            AutofacDependencyResolver
                .Current.GetService<IStringLocalizer<AbpWebResource>>()
                .GetString("ValidationError")
        )
        {
            ValidationErrors = validationErrors.ToArray(),
        };

        return new AjaxResponse(errorInfo);
    }
}
