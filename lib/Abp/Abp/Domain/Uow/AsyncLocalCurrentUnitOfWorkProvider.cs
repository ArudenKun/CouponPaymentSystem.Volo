using System.Diagnostics.CodeAnalysis;
using Abp.DependencyInjection;

namespace Abp.Domain.Uow;

/// <summary>
/// CallContext implementation of <see cref="ICurrentUnitOfWorkProvider"/>.
/// This is the default implementation.
/// </summary>
internal class AsyncLocalCurrentUnitOfWorkProvider
    : ICurrentUnitOfWorkProvider,
        ITransientDependency
{
    private static readonly AsyncLocal<LocalUowWrapper?> AsyncLocalUow = new();

    /// <inheritdoc />
    public IUnitOfWork? Current
    {
        get => GetCurrentUow();
        set => SetCurrentUow(value);
    }

    [SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
    private static IUnitOfWork? GetCurrentUow()
    {
        var uow = AsyncLocalUow.Value?.UnitOfWork;
        if (uow == null)
        {
            return null;
        }

        if (uow.IsDisposed)
        {
            AsyncLocalUow.Value = null;
            return null;
        }

        return uow;
    }

    private static void SetCurrentUow(IUnitOfWork? value)
    {
        lock (AsyncLocalUow)
        {
            if (value == null)
            {
                if (AsyncLocalUow.Value == null)
                {
                    return;
                }

                if (AsyncLocalUow.Value.UnitOfWork?.Outer == null)
                {
                    AsyncLocalUow.Value.UnitOfWork = null;
                    AsyncLocalUow.Value = null;
                    return;
                }

                AsyncLocalUow.Value.UnitOfWork = AsyncLocalUow.Value.UnitOfWork.Outer;
            }
            else
            {
                if (AsyncLocalUow.Value?.UnitOfWork == null)
                {
                    if (AsyncLocalUow.Value != null)
                    {
                        AsyncLocalUow.Value.UnitOfWork = value;
                    }

                    AsyncLocalUow.Value = new LocalUowWrapper(value);
                    return;
                }

                value.Outer = AsyncLocalUow.Value.UnitOfWork;
                AsyncLocalUow.Value.UnitOfWork = value;
            }
        }
    }

    private class LocalUowWrapper
    {
        public IUnitOfWork? UnitOfWork { get; set; }

        public LocalUowWrapper(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
