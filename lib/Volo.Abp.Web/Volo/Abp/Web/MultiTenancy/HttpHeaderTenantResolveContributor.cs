using System.Web;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Web.MultiTenancy;

public class HttpHeaderTenantResolveContributor : HttpTenantResolveContributorBase
{
    public const string ContributorName = "Header";

    public override string Name => ContributorName;

    protected override Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(
        ITenantResolveContext context,
        HttpContext httpContext
    )
    {
        if (httpContext.Request.Headers.Count == 0)
        {
            return Task.FromResult<string?>(null);
        }

        var tenantIdKey = context.GetAbpWebMultiTenancyOptions().TenantKey;
        var tenantIdHeader = httpContext.Request.Headers[tenantIdKey];
        if (tenantIdHeader.IsNullOrEmpty())
        {
            return Task.FromResult<string?>(null);
        }

        return Task.FromResult<string?>(tenantIdHeader);
    }
}
