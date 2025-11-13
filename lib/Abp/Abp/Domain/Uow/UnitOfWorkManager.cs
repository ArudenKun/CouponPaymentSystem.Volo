using System.Transactions;
using Abp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Abp.Domain.Uow;

/// <summary>
/// Unit of work manager.
/// </summary>
internal class UnitOfWorkManager : IUnitOfWorkManager, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
    private readonly UnitOfWorkDefaultOptions _defaultOptions;

    public IActiveUnitOfWork? Current => _currentUnitOfWorkProvider.Current;

    public UnitOfWorkManager(
        IServiceProvider serviceProvider,
        ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
        IOptions<UnitOfWorkDefaultOptions> defaultOptions
    )
    {
        _serviceProvider = serviceProvider;
        _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        _defaultOptions = defaultOptions.Value;
    }

    public IUnitOfWorkCompleteHandle Begin()
    {
        return Begin(new UnitOfWorkOptions());
    }

    public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
    {
        return Begin(new UnitOfWorkOptions { Scope = scope });
    }

    public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
    {
        options.FillDefaultsForNonProvidedOptions(_defaultOptions);

        var outerUow = _currentUnitOfWorkProvider.Current;

        if (options.Scope == TransactionScopeOption.Required && outerUow != null)
        {
            return outerUow.Options?.Scope == TransactionScopeOption.Suppress
                ? new InnerSuppressUnitOfWorkCompleteHandle(outerUow)
                : new InnerUnitOfWorkCompleteHandle();
        }

        var uow = _serviceProvider.GetRequiredServiceAsDisposable<IUnitOfWork>();

        uow.Service.Completed += (_, _) =>
        {
            _currentUnitOfWorkProvider.Current = null;
        };

        uow.Service.Failed += (_, _) =>
        {
            _currentUnitOfWorkProvider.Current = null;
        };

        uow.Service.Disposed += (_, _) =>
        {
            uow.Dispose();
        };

        //Inherit filters from outer UOW
        if (outerUow != null)
        {
            options.FillOuterUowFiltersForNonProvidedOptions(outerUow.Filters.ToList());
        }

        uow.Service.Begin(options);

        //Inherit tenant from outer UOW
        if (outerUow != null)
        {
            uow.Service.SetTenantId(outerUow.GetTenantId(), false);
        }

        _currentUnitOfWorkProvider.Current = uow.Service;

        return uow.Service;
    }
}
