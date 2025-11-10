using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Uow;
using Volo.Abp.Web.Mvc.Extensions;

namespace Volo.Abp.Web.Mvc.Uow;

internal class AbpMvcUowFilter : IActionFilter, ITransientDependency
{
    public const string UowHttpContextKey = "__AbpUnitOfWork";

    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly AbpMvcOptions _mvcOptions;
    private readonly AbpUnitOfWorkDefaultOptions _unitOfWorkDefaultOptions;

    public AbpMvcUowFilter(
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<AbpMvcOptions> mvcConfiguration,
        IOptions<AbpUnitOfWorkDefaultOptions> unitOfWorkDefaultOptions
    )
    {
        _unitOfWorkManager = unitOfWorkManager;
        _mvcOptions = mvcConfiguration.Value;
        _unitOfWorkDefaultOptions = unitOfWorkDefaultOptions.Value;
    }

    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (filterContext.IsChildAction)
        {
            return;
        }

        var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
        if (methodInfo == null)
            return;

        var unitOfWorkAttr =
            UnitOfWorkHelper.GetUnitOfWorkAttributeOrNull(methodInfo)
            ?? _mvcOptions.DefaultUnitOfWorkAttribute;

        if (unitOfWorkAttr.IsDisabled)
            return;

        SetCurrentUow(
            filterContext.HttpContext,
            _unitOfWorkManager.Begin(CreateOptions(filterContext, unitOfWorkAttr))
        );
    }

    public void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (filterContext.IsChildAction)
        {
            return;
        }

        var uow = GetCurrentUow(filterContext.HttpContext);
        if (uow == null)
        {
            return;
        }

        try
        {
            if (filterContext.Exception == null)
            {
                // ReSharper disable once AccessToDisposedClosure
                AsyncHelper.RunSync(() => uow.CompleteAsync());
            }
        }
        finally
        {
            uow.Dispose();
            SetCurrentUow(filterContext.HttpContext, null);
        }
    }

    private static IUnitOfWork? GetCurrentUow(HttpContextBase httpContext)
    {
        return httpContext.Items[UowHttpContextKey] as IUnitOfWork;
    }

    private static void SetCurrentUow(HttpContextBase httpContext, IUnitOfWork? uow)
    {
        httpContext.Items[UowHttpContextKey] = uow;
    }

    private AbpUnitOfWorkOptions CreateOptions(
        ActionExecutingContext context,
        UnitOfWorkAttribute? unitOfWorkAttribute
    )
    {
        var options = new AbpUnitOfWorkOptions();

        unitOfWorkAttribute?.SetOptions(options);

        if (unitOfWorkAttribute?.IsTransactional == null)
        {
            options.IsTransactional = _unitOfWorkDefaultOptions.CalculateIsTransactional(
                autoValue: !string.Equals(
                    context.HttpContext.Request.HttpMethod,
                    nameof(HttpVerbs.Get),
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }

        return options;
    }
}
