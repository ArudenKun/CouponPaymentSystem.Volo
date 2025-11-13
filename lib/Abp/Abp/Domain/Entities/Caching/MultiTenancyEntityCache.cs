using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using JetBrains.Annotations;

namespace Abp.Domain.Entities.Caching;

[PublicAPI]
public abstract class MultiTenancyEntityCache<TEntity, TCacheItem, TPrimaryKey>
    : EntityCacheBase<TEntity, TCacheItem, TPrimaryKey>,
        IEventHandler<EntityChangedEventData<TEntity>>,
        IMultiTenancyEntityCache<TCacheItem, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    public required IAbpSession AbpSession { protected get; init; }

    public ITypedCache<string, TCacheItem> InternalCache =>
        CacheManager.GetCache<string, TCacheItem>(CacheName);

    public MultiTenancyEntityCache(string? cacheName = null)
        : base(cacheName) { }

    public override TCacheItem Get(TPrimaryKey id)
    {
        return InternalCache.Get(GetCacheKey(id), () => GetCacheItemFromDataSource(id));
    }

    public override Task<TCacheItem> GetAsync(TPrimaryKey id)
    {
        return InternalCache.GetAsync(
            GetCacheKey(id),
            async () => await GetCacheItemFromDataSourceAsync(id)
        );
    }

    public virtual void HandleEvent(EntityChangedEventData<TEntity> eventData)
    {
        InternalCache.Remove(GetCacheKey(eventData.Entity));
    }

    protected virtual Guid? GetCurrentTenantId()
    {
        if (UnitOfWorkManager.Current != null)
        {
            return UnitOfWorkManager.Current.GetTenantId();
        }

        return AbpSession.TenantId;
    }

    public virtual string GetCacheKey(TPrimaryKey id)
    {
        return GetCacheKey(id, GetCurrentTenantId());
    }

    public virtual string GetCacheKey(TPrimaryKey id, Guid? tenantId)
    {
        return id + "@" + (tenantId ?? Guid.Empty);
    }

    protected abstract string GetCacheKey(TEntity entity);

    public override string ToString()
    {
        return $"MultiTenancyEntityCache {CacheName}";
    }
}
