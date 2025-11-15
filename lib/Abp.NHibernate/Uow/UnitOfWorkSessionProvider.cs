using Abp.Dependency;
using Abp.Domain.Uow;
using NHibernate;

namespace Abp.NHibernate.Uow;

internal class UnitOfWorkSessionProvider : ISessionProvider, ITransientDependency
{
    public ISession Session => _unitOfWorkProvider.Current.GetSession();

    private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;

    public UnitOfWorkSessionProvider(ICurrentUnitOfWorkProvider unitOfWorkProvider)
    {
        _unitOfWorkProvider = unitOfWorkProvider;
    }
}
