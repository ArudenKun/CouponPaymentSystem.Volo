using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using JetBrains.Annotations;

namespace Abp.Domain.Entities.Caching;

[PublicAPI]
public abstract class EntityCacheBase<TEntity, TCacheItem, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    public TCacheItem this[TPrimaryKey id] => Get(id);

    public string CacheName { get; }

    public required IObjectMapper ObjectMapper { protected get; init; }

    public required ICacheManager CacheManager { protected get; init; }

    public required IRepository<TEntity, TPrimaryKey> Repository { protected get; init; }

    public required IUnitOfWorkManager UnitOfWorkManager { protected get; init; }

    public EntityCacheBase(string? cacheName = null)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CacheName = cacheName ?? GenerateDefaultCacheName();
    }

    public abstract TCacheItem Get(TPrimaryKey id);

    public abstract Task<TCacheItem> GetAsync(TPrimaryKey id);

    protected virtual TCacheItem GetCacheItemFromDataSource(TPrimaryKey id)
    {
        return MapToCacheItem(GetEntityFromDataSource(id));
    }

    protected virtual async Task<TCacheItem> GetCacheItemFromDataSourceAsync(TPrimaryKey id)
    {
        return MapToCacheItem(await GetEntityFromDataSourceAsync(id));
    }

    protected virtual TEntity GetEntityFromDataSource(TPrimaryKey id)
    {
        return UnitOfWorkManager.WithUnitOfWork(() => Repository.FirstOrDefault(id));
    }

    protected virtual async Task<TEntity> GetEntityFromDataSourceAsync(TPrimaryKey id)
    {
        return await UnitOfWorkManager.WithUnitOfWorkAsync(async () =>
            await Repository.FirstOrDefaultAsync(id)
        );
    }

    protected virtual TCacheItem MapToCacheItem(TEntity entity)
    {
        if (ObjectMapper is NullObjectMapper)
        {
            throw new AbpException(
                string.Format(
                    "MapToCacheItem method should be overridden or IObjectMapper should be implemented in order to map {0} to {1}",
                    typeof(TEntity),
                    typeof(TCacheItem)
                )
            );
        }

        return ObjectMapper.Map<TCacheItem>(entity);
    }

    protected virtual string GenerateDefaultCacheName()
    {
        return GetType().FullName ?? GetType().GetFullNameWithAssemblyName();
    }
}
