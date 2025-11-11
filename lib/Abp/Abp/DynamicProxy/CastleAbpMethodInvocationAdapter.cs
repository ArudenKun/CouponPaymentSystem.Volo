using Castle.DynamicProxy;

namespace Abp.DynamicProxy;

public class CastleAbpMethodInvocationAdapter
    : CastleAbpMethodInvocationAdapterBase,
        IAbpMethodInvocation
{
    protected IInvocationProceedInfo ProceedInfo { get; }
    protected Func<IInvocation, IInvocationProceedInfo, Task> Proceed { get; }

    public CastleAbpMethodInvocationAdapter(
        IInvocation invocation,
        IInvocationProceedInfo proceedInfo,
        Func<IInvocation, IInvocationProceedInfo, Task> proceed
    )
        : base(invocation)
    {
        ProceedInfo = proceedInfo;
        Proceed = proceed;
    }

    public override async Task ProceedAsync()
    {
        await Proceed(Invocation, ProceedInfo);
    }
}
