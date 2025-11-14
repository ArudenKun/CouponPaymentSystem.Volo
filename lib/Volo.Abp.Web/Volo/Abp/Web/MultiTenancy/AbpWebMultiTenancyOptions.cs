using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Web.MultiTenancy;

public class AbpWebMultiTenancyOptions
{
    /// <summary>
    /// Default: <see cref="TenantResolverConsts.DefaultTenantKey"/>.
    /// </summary>
    public string TenantKey { get; set; } = TenantResolverConsts.DefaultTenantKey;
}
