using System.Security.Claims;
using Abp.Web.Mvc.Views;

namespace Presentation.Views;

public abstract class AppWebViewPage : AppWebViewPage<dynamic>;

public abstract class AppWebViewPage<TModel> : AbpWebViewPage<TModel>
{
    public new ClaimsPrincipal User => (ClaimsPrincipal)base.User;
}
