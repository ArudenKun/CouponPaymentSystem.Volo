namespace Abp.Domain.Entities.Caching;

public class MustHaveTenantEntityCache<TEntity, TCacheItem>
    : MustHaveTenantEntityCache<TEntity, TCacheItem, Guid>,
        IMultiTenancyEntityCache<TCacheItem>
    where TEntity : class, IEntity<Guid>, IMustHaveTenant
{
    public MustHaveTenantEntityCache(string? cacheName = null)
        : base(cacheName) { }
}

public class MustHaveTenantEntityCache<TEntity, TCacheItem, TPrimaryKey>
    : MultiTenancyEntityCache<TEntity, TCacheItem, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>, IMustHaveTenant
{
    public MustHaveTenantEntityCache(string? cacheName = null)
        : base(cacheName) { }

    protected override string GetCacheKey(TEntity entity)
    {
        return GetCacheKey(entity.Id, entity.TenantId);
    }

    public override string ToString()
    {
        return $"MustHaveTenantEntityCache {CacheName}";
    }
}
