using System.Net;
using System.Reflection;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Json;
using Volo.Abp.Web.Models;
using Volo.Abp.Web.Mvc.Controllers.Results;
using Volo.Abp.Web.Mvc.Extensions;
using Volo.Abp.Web.Security;

namespace Volo.Abp.Web.Mvc.Security;

public class AbpAntiForgeryMvcFilter : IAuthorizationFilter, ITransientDependency
{
    private readonly IAbpAntiForgeryManager _abpAntiForgeryManager;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly AbpWebMvcOptions _abpWebMvcOptions;
    private readonly AbpWebAntiForgeryOptions _abpWebAntiForgeryOptions;
    private readonly ILogger<AbpAntiForgeryMvcFilter> _logger;

    public AbpAntiForgeryMvcFilter(
        IAbpAntiForgeryManager abpAntiForgeryManager,
        IJsonSerializer jsonSerializer,
        IOptions<AbpWebMvcOptions> abpWebMvcOptions,
        IOptions<AbpWebAntiForgeryOptions> abpWebAntiForgeryOptions,
        ILogger<AbpAntiForgeryMvcFilter> logger
    )
    {
        _abpAntiForgeryManager = abpAntiForgeryManager;
        _jsonSerializer = jsonSerializer;
        _logger = logger;
        _abpWebMvcOptions = abpWebMvcOptions.Value;
        _abpWebAntiForgeryOptions = abpWebAntiForgeryOptions.Value;
    }

    public void OnAuthorization(AuthorizationContext context)
    {
        var methodInfo = context.ActionDescriptor.GetMethodInfoOrNull();
        if (methodInfo == null)
        {
            return;
        }

        var httpVerb = HttpMethodHelper.ConvertToHttpMethod(context.HttpContext.Request.HttpMethod);
        if (
            !_abpAntiForgeryManager.ShouldValidate(
                _abpWebAntiForgeryOptions,
                methodInfo,
                httpVerb,
                _abpWebMvcOptions.IsAutomaticAntiForgeryValidationEnabled
            )
        )
        {
            return;
        }

        if (!_abpAntiForgeryManager.IsValid(context.HttpContext))
        {
            CreateErrorResponse(context, methodInfo, "Empty or invalid anti forgery header token.");
        }
    }

    private void CreateErrorResponse(
        AuthorizationContext context,
        MethodInfo methodInfo,
        string message
    )
    {
        _logger.LogWarning(message);
        _logger.LogWarning("Requested URI: {Uri} ", context.HttpContext.Request.Url);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.HttpContext.Response.StatusDescription = message;
        var isJsonResult = MethodInfoHelper.IsJsonResult(methodInfo);

        if (isJsonResult)
        {
            context.Result = CreateUnAuthorizedJsonResult(message);
        }
        else
        {
            context.Result = CreateUnAuthorizedNonJsonResult(context, message);
        }

        if (isJsonResult || context.HttpContext.Request.IsAjaxRequest())
        {
            context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
        }
    }

    protected virtual AbpJsonResult CreateUnAuthorizedJsonResult(string message)
    {
        return new AbpJsonResult(new AjaxResponse(new ErrorInfo(message), true))
        {
            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            JsonSerializer = _jsonSerializer,
        };
    }

    protected virtual HttpStatusCodeResult CreateUnAuthorizedNonJsonResult(
        AuthorizationContext filterContext,
        string message
    )
    {
        return new HttpStatusCodeResult(filterContext.HttpContext.Response.StatusCode, message);
    }
}
