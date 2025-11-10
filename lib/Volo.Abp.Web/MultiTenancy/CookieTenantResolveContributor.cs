using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Web.MultiTenancy;

public class CookieTenantResolveContributor : HttpTenantResolveContributorBase
{
    public const string ContributorName = "Cookie";

    public override string Name => ContributorName;

    protected override Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(
        ITenantResolveContext context,
        HttpContext httpContext
    )
    {
        var options = context
            .ServiceProvider.GetRequiredService<IOptions<AbpWebMultiTenancyOptions>>()
            .Value;

        var cookie = HttpContext.Current?.Request.Cookies[options.TenantKey];
        if (cookie == null || cookie.Value.IsNullOrEmpty())
        {
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult<string?>(cookie.Value);
    }
}
