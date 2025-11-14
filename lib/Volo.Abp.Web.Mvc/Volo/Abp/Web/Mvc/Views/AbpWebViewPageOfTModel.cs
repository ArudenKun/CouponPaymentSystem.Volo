using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Threading;
using Volo.Abp.Web.Security;

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

    public required IServiceProvider ServiceProvider { protected get; init; }

    /// <summary>
    /// Constructor.
    /// </summary>
    protected AbpWebViewPage() { }

    /// <summary>
    /// Checks if current user is granted for a permission.
    /// </summary>
    /// <param name="permissionName">Name of the permission</param>
    protected virtual bool IsGranted(string permissionName) =>
        AsyncHelper.RunSync(() =>
            ServiceProvider.GetRequiredService<IPermissionChecker>().IsGrantedAsync(permissionName)
        );

    protected virtual void SetAntiForgeryCookie()
    {
        ServiceProvider.GetRequiredService<IAbpAntiForgeryManager>().SetCookie(Context);
    }
}
