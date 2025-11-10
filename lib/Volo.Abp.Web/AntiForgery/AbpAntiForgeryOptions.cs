using System.Diagnostics.CodeAnalysis;
using System.Web;
using JetBrains.Annotations;

namespace Volo.Abp.Web.AntiForgery;

public class AbpAntiForgeryOptions
{
    public string TokenCookieName { get; set; } = "XSRF-TOKEN";

    public string TokenHeaderName { get; set; } = "X-XSRF-TOKEN";

    public string AuthorizationCookieName { get; set; } = ".AspNet.ApplicationCookie";

    public string AuthorizationCookieApplicationScheme { get; set; } = "Identity.Application";
}
