using System.Web;
using System.Web.Mvc;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Threading;
using Volo.Abp.Web.AntiForgery;
using Volo.Abp.Web.Mvc.AntiForgery;

namespace Volo.Abp.Web.Mvc.Views;

/// <summary>
/// Base class for all views in Abp system.
/// </summary>
/// <typeparam name="TModel">Type of the View Model</typeparam>
public abstract class AbpWebViewPage<TModel> : WebViewPage<TModel>
{
    /// <summary>
    /// Gets the root path of the application.
    /// </summary>
    public string ApplicationPath
    {
        get
        {
            var appPath = HttpContext.Current.Request.ApplicationPath;
            if (appPath == null)
            {
                return "/";
            }

            appPath = appPath.EnsureEndsWith('/');

            return appPath;
        }
    }

    public required IPermissionChecker PermissionChecker { protected get; init; }

    public required IAbpAntiForgeryManager AntiForgeryManager { protected get; init; }

    /// <summary>
    /// Constructor.
    /// </summary>
    protected AbpWebViewPage() { }

    /// <summary>
    /// Checks if current user is granted for a permission.
    /// </summary>
    /// <param name="permissionName">Name of the permission</param>
    protected virtual bool IsGranted(string permissionName) =>
        AsyncHelper.RunSync(() => PermissionChecker.IsGrantedAsync(permissionName));

    protected virtual void SetAntiForgeryCookie()
    {
        AntiForgeryManager.SetCookie(Context);
    }
}
