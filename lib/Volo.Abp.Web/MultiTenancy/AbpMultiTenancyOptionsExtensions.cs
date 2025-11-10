using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Web.MultiTenancy;

public static class AbpMultiTenancyOptionsExtensions
{
    public static void AddDomainTenantResolver(
        this AbpTenantResolveOptions options,
        string domainFormat
    )
    {
        options.TenantResolvers.InsertAfter(
            r => r is CurrentUserTenantResolveContributor,
            new DomainTenantResolveContributor(domainFormat)
        );
    }
}
