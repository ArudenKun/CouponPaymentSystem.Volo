using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.NHibernate.Repositories;

/// <summary>
/// A shortcut of <see cref="NhNhRepositoryBase{TEntity,TPrimaryKey}"/> for most used primary key type (<see cref="int"/>).
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public class NhNhRepositoryBase<TEntity> : NhNhRepositoryBase<TEntity, int>, IRepository<TEntity>
    where TEntity : class, IEntity<int>
{
    /// <summary>
    /// Creates a new <see cref="NhNhRepositoryBase{TEntity,TPrimaryKey}"/> object.
    /// </summary>
    /// <param name="sessionProvider">A session provider to obtain session for database operations</param>
    public NhNhRepositoryBase(ISessionProvider sessionProvider)
        : base(sessionProvider) { }
}
