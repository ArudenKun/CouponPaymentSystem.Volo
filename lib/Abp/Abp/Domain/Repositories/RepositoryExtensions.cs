using System.Linq.Expressions;
using Abp.Domain.Entities;
using Abp.Domain.Uow;
using Abp.DynamicProxy;

namespace Abp.Domain.Repositories;

public static class RepositoryExtensions
{
    public static async Task EnsureCollectionLoadedAsync<TEntity, TPrimaryKey, TProperty>(
        this IRepository<TEntity, TPrimaryKey> repository,
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
        CancellationToken cancellationToken = default(CancellationToken)
    )
        where TEntity : class, IEntity<TPrimaryKey>
        where TProperty : class
    {
        if (ProxyHelper.UnProxy(repository) is ISupportsExplicitLoading<TEntity, TPrimaryKey> repo)
        {
            await repo.EnsureCollectionLoadedAsync(entity, collectionExpression, cancellationToken);
        }
    }

    public static void EnsureCollectionLoaded<TEntity, TPrimaryKey, TProperty>(
        this IRepository<TEntity, TPrimaryKey> repository,
        TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression,
        CancellationToken cancellationToken = default(CancellationToken)
    )
        where TEntity : class, IEntity<TPrimaryKey>
        where TProperty : class
    {
        if (ProxyHelper.UnProxy(repository) is ISupportsExplicitLoading<TEntity, TPrimaryKey> repo)
        {
            repo.EnsureCollectionLoaded(entity, collectionExpression, cancellationToken);
        }
    }

    public static async Task EnsurePropertyLoadedAsync<TEntity, TPrimaryKey, TProperty>(
        this IRepository<TEntity, TPrimaryKey> repository,
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default
    )
        where TEntity : class, IEntity<TPrimaryKey>
        where TProperty : class
    {
        if (ProxyHelper.UnProxy(repository) is ISupportsExplicitLoading<TEntity, TPrimaryKey> repo)
        {
            await repo.EnsurePropertyLoadedAsync(entity, propertyExpression, cancellationToken);
        }
    }

    public static void EnsurePropertyLoaded<TEntity, TPrimaryKey, TProperty>(
        this IRepository<TEntity, TPrimaryKey> repository,
        TEntity entity,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        CancellationToken cancellationToken = default(CancellationToken)
    )
        where TEntity : class, IEntity<TPrimaryKey>
        where TProperty : class
    {
        if (ProxyHelper.UnProxy(repository) is ISupportsExplicitLoading<TEntity, TPrimaryKey> repo)
        {
            repo.EnsurePropertyLoaded(entity, propertyExpression, cancellationToken);
        }
    }

    public static IServiceProvider GetIocResolver<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        if (ProxyHelper.UnProxy(repository) is AbpRepositoryBase<TEntity, TPrimaryKey> repo)
        {
            return repo.IocResolver;
        }

        throw new ArgumentException(
            $"Given {nameof(repository)} is not inherited from {typeof(AbpRepositoryBase<TEntity, TPrimaryKey>).AssemblyQualifiedName}"
        );
    }

    public static void HardDelete<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        TEntity entity
    )
        where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
    {
        HardDelete(repository, null, entity);
    }

    public static void HardDelete<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        Expression<Func<TEntity, bool>> predicate
    )
        where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
    {
        HardDelete(repository, predicate, null);
    }

    private static void HardDelete<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        Expression<Func<TEntity, bool>>? predicate,
        TEntity? entity
    )
        where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
    {
        var currentUnitOfWork = GetCurrentUnitOfWorkOrThrowException(repository);
        var hardDeleteEntities =
            (HashSet<string>)
                currentUnitOfWork.Items.GetOrAdd(
                    UnitOfWorkExtensionDataTypes.HardDelete,
                    () => new HashSet<string>()
                );

        var tenantId = currentUnitOfWork.GetTenantId();

        if (predicate != null)
        {
            foreach (var e in repository.GetAll().Where(predicate).ToList())
            {
                var hardDeleteKey = EntityHelper.GetHardDeleteKey(e, tenantId);

                hardDeleteEntities.Add(hardDeleteKey);

                repository.Delete(e);
            }
        }

        if (entity != null)
        {
            var hardDeleteKey = EntityHelper.GetHardDeleteKey(entity, tenantId);

            hardDeleteEntities.Add(hardDeleteKey);

            repository.Delete(entity);
        }
    }

    public static async Task HardDeleteAsync<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        TEntity entity
    )
        where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
    {
        await HardDeleteAsync(repository, null, entity);
    }

    public static async Task HardDeleteAsync<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        Expression<Func<TEntity, bool>> predicate
    )
        where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
    {
        await HardDeleteAsync(repository, predicate, null);
    }

    private static async Task HardDeleteAsync<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        Expression<Func<TEntity, bool>>? predicate,
        TEntity? entity
    )
        where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
    {
        var currentUnitOfWork = GetCurrentUnitOfWorkOrThrowException(repository);
        var hardDeleteEntities =
            (HashSet<string>)
                currentUnitOfWork.Items.GetOrAdd(
                    UnitOfWorkExtensionDataTypes.HardDelete,
                    () => new HashSet<string>()
                );

        var tenantId = currentUnitOfWork.GetTenantId();

        if (predicate != null)
        {
            foreach (var e in (await repository.GetAllAsync()).Where(predicate).ToList())
            {
                var hardDeleteKey = EntityHelper.GetHardDeleteKey(e, tenantId);

                hardDeleteEntities.Add(hardDeleteKey);

                await repository.DeleteAsync(e);
            }
        }

        if (entity != null)
        {
            var hardDeleteKey = EntityHelper.GetHardDeleteKey(entity, tenantId);

            hardDeleteEntities.Add(hardDeleteKey);

            await repository.DeleteAsync(entity);
        }
    }

    private static IActiveUnitOfWork GetCurrentUnitOfWorkOrThrowException<TEntity, TPrimaryKey>(
        IRepository<TEntity, TPrimaryKey> repository
    )
        where TEntity : class, IEntity<TPrimaryKey>, ISoftDelete
    {
        if (ProxyHelper.UnProxy(repository) is not IRepository<TEntity, TPrimaryKey> repo)
        {
            throw new ArgumentException(
                $"Given {nameof(repository)} is not inherited from {typeof(IRepository<TEntity, TPrimaryKey>).AssemblyQualifiedName}"
            );
        }

        var currentUnitOfWork = ((IUnitOfWorkManagerAccessor)repo).UnitOfWorkManager.Current;
        if (currentUnitOfWork == null)
        {
            throw new AbpException(
                $"There is no unit of work in the current context, The hard delete function can only be used in a unit of work."
            );
        }

        return currentUnitOfWork;
    }
}
