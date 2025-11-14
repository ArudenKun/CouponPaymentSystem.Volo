using System.Web;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Text.Formatting;

namespace Volo.Abp.Web.MultiTenancy;

public class DomainTenantResolveContributor : HttpTenantResolveContributorBase, ITransientDependency
{
    public const string ContributorName = "Domain";

    public override string Name => ContributorName;

    private static readonly string[] ProtocolPrefixes = { "http://", "https://" };

    private readonly string _domainFormat;

    public DomainTenantResolveContributor(string domainFormat)
    {
        _domainFormat = domainFormat.RemovePreFix(ProtocolPrefixes);
    }

    protected override Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(
        ITenantResolveContext context,
        HttpContext httpContext
    )
    {
        var hostName = httpContext.Request.Url.Host.RemovePreFix(ProtocolPrefixes);
        var extractResult = FormattedStringValueExtracter.Extract(
            hostName,
            _domainFormat,
            ignoreCase: true
        );

        context.Handled = true;
        return Task.FromResult(extractResult.IsMatch ? extractResult.Matches[0].Value : null);
    }
}
