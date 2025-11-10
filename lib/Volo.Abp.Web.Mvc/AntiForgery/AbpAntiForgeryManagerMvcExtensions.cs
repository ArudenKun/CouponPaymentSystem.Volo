using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;
using Volo.Abp.Web.AntiForgery;

namespace Volo.Abp.Web.Mvc.AntiForgery;

public static class AbpAntiForgeryManagerMvcExtensions
{
    public static void SetCookie(
        this IAbpAntiForgeryManager manager,
        HttpContextBase context,
        IIdentity? identity = null
    )
    {
        if (identity != null)
        {
            context.User = new ClaimsPrincipal(identity);
        }

        context.Response.Cookies.Add(
            new HttpCookie(manager.Options.TokenCookieName, manager.GenerateToken())
        );
    }

    public static bool IsValid(this IAbpAntiForgeryManager manager, HttpContextBase context)
    {
        var authCookieValue = GetCookieValue(context, manager.Options.AuthorizationCookieName);
        var antiForgeryCookieValue = GetCookieValue(context, AntiForgeryConfig.CookieName);

        if (antiForgeryCookieValue.IsNullOrEmpty())
        {
            return authCookieValue.IsNullOrEmpty();
        }

        var formOrHeaderValue = manager.Options.GetFormOrHeaderValue(context);
        if (formOrHeaderValue.IsNullOrEmpty())
        {
            return false;
        }

        return manager
            .As<IAbpAntiForgeryValidator>()
            .IsValid(antiForgeryCookieValue, formOrHeaderValue);
    }

    private static string? GetCookieValue(HttpContextBase context, string cookieName)
    {
        var cookie = context.Request.Cookies[cookieName];
        return cookie?.Value;
    }

    private static string? GetFormOrHeaderValue(
        this AbpAntiForgeryOptions options,
        HttpContextBase context
    )
    {
        var formValue = context.Request.Form["__RequestVerificationToken"];
        if (!formValue.IsNullOrEmpty())
        {
            return formValue;
        }

        var headerValues = context.Request.Headers.GetValues(options.TokenHeaderName);
        if (headerValues == null)
        {
            return null;
        }

        var headersArray = headerValues.ToArray();
        if (!headersArray.Any())
        {
            return null;
        }

        return headersArray.Last().Split(", ").Last();
    }
}
