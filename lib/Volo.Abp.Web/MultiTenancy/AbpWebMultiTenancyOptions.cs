namespace Volo.Abp.Web.MultiTenancy;

public class AbpWebMultiTenancyOptions
{
    /// <summary>
    /// TenantId resolve key
    /// Default value: "Abp.TenantId"
    /// </summary>
    public string TenantKey { get; set; } = "Abp.TenantId";
}
