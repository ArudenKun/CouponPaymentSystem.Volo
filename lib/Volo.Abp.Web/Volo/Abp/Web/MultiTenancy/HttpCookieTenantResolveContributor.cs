using System.Web;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Web.MultiTenancy;

public class HttpCookieTenantResolveContributor : HttpTenantResolveContributorBase
{
    public override string Name => "Cookie";

    protected override Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(
        ITenantResolveContext context,
        HttpContext httpContext
    )
    {
        var cookie = HttpContext
            .Current
            ?.Request
            .Cookies[context.GetAbpWebMultiTenancyOptions().TenantKey];
        if (cookie == null || cookie.Value.IsNullOrEmpty())
        {
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult<string?>(cookie.Value ?? string.Empty);
    }
}
