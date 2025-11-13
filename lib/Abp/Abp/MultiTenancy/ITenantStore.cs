using JetBrains.Annotations;

namespace Abp.MultiTenancy;

public interface ITenantStore
{
    TenantInfo? Find(int tenantId);

    TenantInfo? Find([NotNull] string tenancyName);
}
