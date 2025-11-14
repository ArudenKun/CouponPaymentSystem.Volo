namespace Volo.Abp.Web.Security;

public class AbpAntiForgeryOptions
{
    public AbpAntiForgeryOptions()
    {
        TokenCookieName = "XSRF-TOKEN";
        TokenHeaderName = "X-XSRF-TOKEN";
        AuthorizationCookieName = ".AspNet.ApplicationCookie";
        AuthorizationCookieApplicationScheme = "Identity.Application";
    }

    public string TokenCookieName { get; set; }

    public string TokenHeaderName { get; set; }

    public string AuthorizationCookieName { get; set; }

    public string AuthorizationCookieApplicationScheme { get; set; }
}
