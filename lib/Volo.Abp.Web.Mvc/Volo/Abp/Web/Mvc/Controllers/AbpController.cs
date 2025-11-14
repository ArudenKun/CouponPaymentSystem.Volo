using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Aspects;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EventBus.Local;
using Volo.Abp.Json;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;
using Volo.Abp.Users;
using Volo.Abp.Validation;
using Volo.Abp.Web.Models;
using Volo.Abp.Web.Mvc.Controllers.Results;
using Volo.Abp.Web.Mvc.Extensions;
using Volo.Abp.Web.Mvc.Models;
using Volo.Abp.Web.Mvc.Validation;

namespace Volo.Abp.Web.Mvc.Controllers;

/// <summary>
/// Base class for all MVC Controllers in Abp system.
/// </summary>
[PublicAPI]
public abstract class AbpController : Controller, IAvoidDuplicateCrossCuttingConcerns
{
    public required ITransientCachedServiceProvider ServiceProvider { protected get; init; }

    protected ICurrentUser CurrentUser => ServiceProvider.GetRequiredService<ICurrentUser>();

    protected ICurrentTenant CurrentTenant => ServiceProvider.GetRequiredService<ICurrentTenant>();

    protected ILoggerFactory LoggerFactory => ServiceProvider.GetRequiredService<ILoggerFactory>();

    protected ILogger Logger =>
        ServiceProvider.GetService<ILogger>(_ => LoggerFactory.CreateLogger(GetType().FullName!));

    /// <summary>
    /// Gets the event bus.
    /// </summary>
    protected ILocalEventBus LocalEventBus => ServiceProvider.GetRequiredService<ILocalEventBus>();

    // /// <summary>
    // /// Reference to the permission manager.
    // /// </summary>
    // public required IPermissionManager PermissionManager { protected get; init; }
    //
    // /// <summary>
    // /// Reference to the setting manager.
    // /// </summary>
    // public required ISettingManager SettingManager { protected get; init; }

    /// <summary>
    /// Reference to the permission checker.
    /// </summary>
    protected IPermissionChecker PermissionChecker =>
        ServiceProvider.GetRequiredService<IPermissionChecker>();

    /// <summary>
    /// Reference to the error info builder.
    /// </summary>
    protected IErrorInfoBuilder ErrorInfoBuilder =>
        ServiceProvider.GetRequiredService<IErrorInfoBuilder>();

    /// <summary>
    /// Reference to <see cref="IUnitOfWorkManager"/>.
    /// </summary>
    protected IUnitOfWorkManager UnitOfWorkManager =>
        ServiceProvider.GetRequiredService<IUnitOfWorkManager>();

    protected AbpWebOptions AbpWebOptions =>
        ServiceProvider.GetRequiredService<IOptions<AbpWebOptions>>().Value;
    protected AbpWebMvcOptions AbpWebMvcOptions =>
        ServiceProvider.GetRequiredService<IOptions<AbpWebMvcOptions>>().Value;

    protected IJsonSerializer JsonSerializer =>
        ServiceProvider.GetRequiredService<IJsonSerializer>();

    protected IClock Clock => ServiceProvider.GetRequiredService<IClock>();

    protected IModelStateValidator ModelValidator =>
        ServiceProvider.GetRequiredService<IModelStateValidator>();

    public List<string> AppliedCrossCuttingConcerns { get; } = [];

    /// <summary>
    /// Gets current unit of work.
    /// </summary>
    protected IUnitOfWork? CurrentUnitOfWork => UnitOfWorkManager.Current;

    /// <summary>
    /// MethodInfo for currently executing action.
    /// </summary>
    private MethodInfo? _currentMethodInfo;

    /// <summary>
    /// WrapResultAttribute for currently executing action.
    /// </summary>
    private WrapResultAttribute? _wrapResultAttribute;

    /// <summary>
    /// Constructor.
    /// </summary>
    protected AbpController() { }

    /// <summary>
    /// Checks if current user is granted for a permission.
    /// </summary>
    /// <param name="permissionName">Name of the permission</param>
    protected Task<bool> IsGrantedAsync(string permissionName)
    {
        return PermissionChecker.IsGrantedAsync(permissionName);
    }

    /// <summary>
    /// Checks if current user is granted for a permission.
    /// </summary>
    /// <param name="permissionName">Name of the permission</param>
    protected bool IsGranted(string permissionName)
    {
        return AsyncHelper.RunSync(() => PermissionChecker.IsGrantedAsync(permissionName));
    }

    /// <summary>
    /// Json the specified data, contentType, contentEncoding and behavior.
    /// </summary>
    /// <param name="data">Data.</param>
    /// <param name="contentType">Content type.</param>
    /// <param name="contentEncoding">Content encoding.</param>
    /// <param name="behavior">Behavior.</param>
    protected override JsonResult Json(
        object data,
        string contentType,
        Encoding contentEncoding,
        JsonRequestBehavior behavior
    )
    {
        if (
            Request.Url != null
            && AbpWebOptions.WrapResultFilters.HasFilterForWrapOnSuccess(
                Request.Url.AbsolutePath,
                out var wrapOnSuccess
            )
        )
        {
            if (!wrapOnSuccess)
            {
                return base.Json(data, contentType, contentEncoding, behavior);
            }

            return AbpJson(data, contentType, contentEncoding, behavior);
        }

        if (_wrapResultAttribute is { WrapOnSuccess: false })
        {
            return base.Json(data, contentType, contentEncoding, behavior);
        }

        return AbpJson(data, contentType, contentEncoding, behavior);
    }

    protected virtual AbpJsonResult AbpJson(
        object? data,
        string? contentType = null,
        Encoding? contentEncoding = null,
        JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet,
        bool wrapResult = true,
        bool camelCase = true,
        bool indented = false
    )
    {
        if (wrapResult)
        {
            if (data == null)
            {
                data = new AjaxResponse();
            }
            else if (!(data is AjaxResponseBase))
            {
                data = new AjaxResponse(data);
            }
        }

        return new AbpJsonResult
        {
            Data = data,
            ContentType = contentType,
            ContentEncoding = contentEncoding,
            JsonRequestBehavior = behavior,
            CamelCase = camelCase,
            Indented = indented,
            JsonSerializer = JsonSerializer,
        };
    }

    #region OnActionExecuting / OnActionExecuted

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        SetCurrentMethodInfoAndWrapResultAttribute(filterContext);
        base.OnActionExecuting(filterContext);
    }

    private void SetCurrentMethodInfoAndWrapResultAttribute(ActionExecutingContext filterContext)
    {
        //Prevent overriding for child actions
        if (_currentMethodInfo is not null)
            return;

        _currentMethodInfo = filterContext.ActionDescriptor.GetMethodInfoOrNull();
        _wrapResultAttribute = ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault(
            _currentMethodInfo!,
            AbpWebMvcOptions.DefaultWrapResultAttribute
        );
    }

    #endregion

    #region Exception handling

    protected override void OnException(ExceptionContext context)
    {
        void HandleError()
        {
            //We handled the exception!
            context.ExceptionHandled = true;

            //Return an error response to the client.
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = GetStatusCodeForException(
                context,
                _wrapResultAttribute?.WrapOnError ?? true
            );

            context.Result = MethodInfoHelper.IsJsonResult(_currentMethodInfo)
                ? GenerateJsonExceptionResult(context)
                : GenerateNonJsonExceptionResult(context);

            // Certain versions of IIS will sometimes use their own error page when
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            context.HttpContext.Response.TrySkipIisCustomErrors = true;

            //Trigger an event, so we can register it.
            LocalEventBus.PublishAsync(
                new AbpException(context.Exception.Message, context.Exception)
            );
        }

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        //If exception handled before, do nothing.
        //If this is child action, exception should be handled by main action.
        if (context.ExceptionHandled || context.IsChildAction)
        {
            base.OnException(context);
            return;
        }

        //Log exception
        if (_wrapResultAttribute == null || _wrapResultAttribute.LogError)
        {
            ServiceProvider
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger($"{GetType().FullName}")
                .LogException(context.Exception);
        }

        // If custom errors are disabled, we need to let the normal ASP.NET exception handler
        // execute so that the user can see useful debugging information.
        if (!context.HttpContext.IsCustomErrorEnabled)
        {
            base.OnException(context);
            return;
        }

        // If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method),
        // ignore it.
        if (new HttpException(null, context.Exception).GetHttpCode() != 500)
        {
            base.OnException(context);
            return;
        }

        if (context.HttpContext.Request.Url != null)
        {
            var url = context.HttpContext.Request.Url.AbsolutePath;
            if (AbpWebOptions.WrapResultFilters.HasFilterForWrapOnError(url, out var wrapOnError))
            {
                if (!wrapOnError)
                {
                    base.OnException(context);
                    context.HttpContext.Response.StatusCode = GetStatusCodeForException(
                        context,
                        false
                    );
                    return;
                }

                HandleError();
                return;
            }
        }

        //Check WrapResultAttribute
        if (_wrapResultAttribute == null || !_wrapResultAttribute.WrapOnError)
        {
            base.OnException(context);
            context.HttpContext.Response.StatusCode = GetStatusCodeForException(
                context,
                _wrapResultAttribute?.WrapOnError ?? true
            );
            return;
        }

        HandleError();
    }

    protected virtual int GetStatusCodeForException(ExceptionContext context, bool wrapOnError)
    {
        if (context.Exception is AbpAuthorizationException)
        {
            return context.HttpContext.User.Identity.IsAuthenticated
                ? (int)HttpStatusCode.Forbidden
                : (int)HttpStatusCode.Unauthorized;
        }

        if (context.Exception is AbpValidationException)
        {
            return (int)HttpStatusCode.BadRequest;
        }

        if (context.Exception is EntityNotFoundException)
        {
            return (int)HttpStatusCode.NotFound;
        }

        if (wrapOnError)
        {
            return (int)HttpStatusCode.InternalServerError;
        }

        return context.HttpContext.Response.StatusCode;
    }

    protected virtual ActionResult GenerateJsonExceptionResult(ExceptionContext context)
    {
        context.HttpContext.Items.Add("IgnoreJsonRequestBehaviorDenyGet", "true");
        return new AbpJsonResult(
            new AjaxResponse(
                ErrorInfoBuilder.BuildForException(context.Exception),
                context.Exception is AbpAuthorizationException
            )
        )
        {
            JsonSerializer = JsonSerializer,
        };
    }

    protected virtual ActionResult GenerateNonJsonExceptionResult(ExceptionContext context)
    {
        return new ViewResult
        {
            ViewName = "Error",
            MasterName = string.Empty,
            ViewData = new ViewDataDictionary<ErrorViewModel>(
                new ErrorViewModel(
                    ErrorInfoBuilder.BuildForException(context.Exception),
                    context.Exception
                )
            ),
            TempData = context.Controller.TempData,
        };
    }

    #endregion
}
