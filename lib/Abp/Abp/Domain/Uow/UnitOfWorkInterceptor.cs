using Abp.DependencyInjection;
using Abp.DynamicProxy;
using Microsoft.Extensions.Options;

namespace Abp.Domain.Uow;

/// <summary>
/// This interceptor is used to manage database connection and transactions.
/// </summary>
internal class UnitOfWorkInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly UnitOfWorkDefaultOptions _unitOfWorkOptions;

    public UnitOfWorkInterceptor(
        IUnitOfWorkManager unitOfWorkManager,
        IOptions<UnitOfWorkDefaultOptions> unitOfWorkOptions
    )
    {
        _unitOfWorkManager = unitOfWorkManager;
        _unitOfWorkOptions = unitOfWorkOptions.Value;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        var unitOfWorkAttr = _unitOfWorkOptions.GetUnitOfWorkAttributeOrNull(invocation.Method);
        if (unitOfWorkAttr == null || unitOfWorkAttr.IsDisabled)
        {
            await invocation.ProceedAsync();
            return;
        }

        using var uow = _unitOfWorkManager.Begin(unitOfWorkAttr.CreateOptions());
        await invocation.ProceedAsync();
        await uow.CompleteAsync();
    }
}
