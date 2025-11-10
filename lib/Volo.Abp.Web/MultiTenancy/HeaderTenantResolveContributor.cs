using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Web.MultiTenancy;

public class HeaderTenantResolveContributor : HttpTenantResolveContributorBase
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

        var tenantIdKey = context
            .ServiceProvider.GetRequiredService<IOptions<AbpWebMultiTenancyOptions>>()
            .Value.TenantKey;

        var tenantIdHeader = httpContext.Request.Headers[tenantIdKey];
        return tenantIdHeader.IsNullOrEmpty()
            ? Task.FromResult<string?>(null)
            : Task.FromResult<string?>(tenantIdHeader);
    }

    protected virtual void Log(ITenantResolveContext context, string text)
    {
        context
            .ServiceProvider.GetRequiredService<ILogger<HeaderTenantResolveContributor>>()
            .LogWarning(text);
    }
}
