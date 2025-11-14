using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Web.Mvc.Extensions;

namespace Volo.Abp.Web.Mvc.Validation;

public class AbpMvcValidationFilter : IActionFilter, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly AbpWebMvcOptions _options;

    public AbpMvcValidationFilter(
        IServiceProvider serviceProvider,
        IOptions<AbpWebMvcOptions> options
    )
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!_options.IsValidationEnabledForControllers)
            return;

        var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
        if (methodInfo == null)
            return;

        using var validator =
            _serviceProvider.GetRequiredServiceAsDisposable<IModelStateValidator>();
        validator.Service.Validate(filterContext.Controller.As<Controller>().ModelState);
    }

    public void OnActionExecuted(ActionExecutedContext filterContext) { }
}
