using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Abp.Domain.Entities.Caching;

public class EntityCache<TEntity, TCacheItem>
    : EntityCache<TEntity, TCacheItem, Guid>,
        IEntityCache<TCacheItem>
    where TEntity : class, IEntity<Guid>
{
    public EntityCache(string? cacheName = null)
        : base(cacheName) { }
}

public class EntityCache<TEntity, TCacheItem, TPrimaryKey>
    : EntityCacheBase<TEntity, TCacheItem, TPrimaryKey>,
        IEventHandler<EntityChangedEventData<TEntity>>,
        IEntityCache<TCacheItem, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    public ITypedCache<TPrimaryKey, TCacheItem> InternalCache =>
        CacheManager.GetCache<TPrimaryKey, TCacheItem>(CacheName);

    public EntityCache(string? cacheName = null)
        : base(cacheName) { }

    public override TCacheItem Get(TPrimaryKey id)
    {
        return InternalCache.Get(id, () => GetCacheItemFromDataSource(id));
    }

    public override Task<TCacheItem> GetAsync(TPrimaryKey id)
    {
        return InternalCache.GetAsync(id, () => GetCacheItemFromDataSourceAsync(id));
    }

    public virtual void HandleEvent(EntityChangedEventData<TEntity> eventData)
    {
        InternalCache.Remove(eventData.Entity.Id);
    }

    public override string ToString()
    {
        return $"EntityCache {CacheName}";
    }
}
