using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Reflection;
using Volo.Abp.Validation;
using Volo.Abp.Web.Mvc.Extensions;

namespace Volo.Abp.Web.Mvc.Validation;

public class AbpMvcValidationFilter : IActionFilter, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpMvcOptions _options;

    public AbpMvcValidationFilter(IServiceProvider serviceProvider, IOptions<AbpMvcOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!_options.IsValidationEnabledForControllers)
        {
            return;
        }

        if (
            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(
                filterContext.ActionDescriptor.GetMethodInfo()
            ) != null
        )
        {
            return;
        }

        if (
            ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableValidationAttribute>(
                filterContext.Controller.GetType()
            ) != null
        )
        {
            return;
        }

        var modelState = filterContext.Controller.As<Controller>().ModelState;
        using var scope = _serviceProvider.CreateScope();
        scope.ServiceProvider.GetRequiredService<IModelStateValidator>().Validate(modelState);
    }

    public void OnActionExecuted(ActionExecutedContext filterContext) { }
}
