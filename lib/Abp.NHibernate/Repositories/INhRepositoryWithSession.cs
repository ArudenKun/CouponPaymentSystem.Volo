using NHibernate;

namespace Abp.NHibernate.Repositories;

internal interface INhRepositoryWithSession
{
    ISession GetSession();
}
