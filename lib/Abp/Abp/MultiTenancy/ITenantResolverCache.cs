using JetBrains.Annotations;

namespace Abp.MultiTenancy;

public interface ITenantResolverCache
{
    TenantResolverCacheItem? Value { get; set; }
}
