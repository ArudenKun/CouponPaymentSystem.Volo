using System.Net;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Web.Models;
using Volo.Abp.Web.Mvc;
using Volo.Abp.Web.Mvc.Controllers.Results;
using Volo.Abp.Web.Mvc.Extensions;
using AllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;

namespace Volo.Abp.Web.Authorization;

public class AbpMvcAuthorizeFilter : IAuthorizationFilter, ITransientDependency
{
    private readonly IAuthorizationService _authorizationHelper;
    private readonly IErrorInfoBuilder _errorInfoBuilder;
    private readonly ILocalEventBus _localEventBus;

    public AbpMvcAuthorizeFilter(
        IAuthorizationHelper authorizationHelper,
        IErrorInfoBuilder errorInfoBuilder,
        ILocalEventBus localEventBus
    )
    {
        _authorizationHelper = authorizationHelper;
        _errorInfoBuilder = errorInfoBuilder;
        _localEventBus = localEventBus;
    }

    public virtual void OnAuthorization(AuthorizationContext filterContext)
    {
        if (
            filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                typeof(AllowAnonymousAttribute),
                true
            )
        )
        {
            return;
        }

        var methodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
        if (methodInfo == null)
        {
            return;
        }

        try
        {
            _authorizationHelper.Authorize(methodInfo, methodInfo.DeclaringType);
        }
        catch (AbpAuthorizationException ex)
        {
            LogHelper.Logger.Warn(ex.ToString(), ex);
            HandleUnauthorizedRequest(filterContext, methodInfo, ex);
        }
    }

    protected virtual void HandleUnauthorizedRequest(
        AuthorizationContext filterContext,
        MethodInfo methodInfo,
        AbpAuthorizationException ex
    )
    {
        filterContext.HttpContext.Response.StatusCode =
            filterContext.RequestContext.HttpContext.User?.Identity?.IsAuthenticated ?? false
                ? (int)HttpStatusCode.Forbidden
                : (int)HttpStatusCode.Unauthorized;

        var isJsonResult = MethodInfoHelper.IsJsonResult(methodInfo);

        if (isJsonResult)
        {
            filterContext.Result = CreateUnAuthorizedJsonResult(ex);
        }
        else
        {
            filterContext.Result = CreateUnAuthorizedNonJsonResult(filterContext, ex);
        }

        if (isJsonResult || filterContext.HttpContext.Request.IsAjaxRequest())
        {
            filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
        }

        _localEventBus.Trigger(this, new AbpHandledExceptionData(ex));
    }

    protected virtual AbpJsonResult CreateUnAuthorizedJsonResult(AbpAuthorizationException ex)
    {
        return new AbpJsonResult(new AjaxResponse(_errorInfoBuilder.BuildForException(ex), true))
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        };
    }

    protected virtual HttpStatusCodeResult CreateUnAuthorizedNonJsonResult(
        AuthorizationContext filterContext,
        AbpAuthorizationException ex
    )
    {
        return new HttpStatusCodeResult(filterContext.HttpContext.Response.StatusCode, ex.Message);
    }
}
