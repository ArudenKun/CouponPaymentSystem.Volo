namespace Abp.Domain.Entities.Caching;

public class MayHaveTenantEntityCache<TEntity, TCacheItem>
    : MayHaveTenantEntityCache<TEntity, TCacheItem, Guid>,
        IMultiTenancyEntityCache<TCacheItem>
    where TEntity : class, IEntity<Guid>, IMayHaveTenant
{
    public MayHaveTenantEntityCache(string? cacheName = null)
        : base(cacheName) { }
}

public class MayHaveTenantEntityCache<TEntity, TCacheItem, TPrimaryKey>
    : MultiTenancyEntityCache<TEntity, TCacheItem, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>, IMayHaveTenant
{
    public MayHaveTenantEntityCache(string? cacheName = null)
        : base(cacheName) { }

    protected override string GetCacheKey(TEntity entity)
    {
        return GetCacheKey(entity.Id, entity.TenantId);
    }

    public override string ToString()
    {
        return $"MayHaveTenantEntityCache {CacheName}";
    }
}
