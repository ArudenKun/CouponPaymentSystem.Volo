using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Reflection;
using NHibernate;

namespace Abp.NHibernate.Repositories;

public static class NhRepositoryExtensions
{
    public static ISession GetSession<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        if (ProxyHelper.UnProxy(repository) is INhRepositoryWithSession repositoryWithSession)
        {
            return repositoryWithSession.GetSession();
        }

        throw new InvalidOperationException(
            "Repository does  not implement IRepositoryWithSession"
        );
    }

    public static IEnumerable<TEntity> InsertRange<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        ICollection<TEntity> entities
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        foreach (var entity in entities)
        {
            repository.Insert(entity);
        }
        return entities;
    }

    public static async Task<IEnumerable<TEntity>> InsertRangeAsync<TEntity, TPrimaryKey>(
        this IRepository<TEntity, TPrimaryKey> repository,
        ICollection<TEntity> entities
    )
        where TEntity : class, IEntity<TPrimaryKey>
    {
        foreach (var entity in entities)
        {
            await repository.InsertAsync(entity);
        }
        return entities;
    }
}
