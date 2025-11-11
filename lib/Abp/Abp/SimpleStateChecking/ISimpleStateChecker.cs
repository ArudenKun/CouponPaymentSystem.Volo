namespace Abp.SimpleStateChecking;

public interface ISimpleStateChecker<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    Task<bool> IsEnabledAsync(SimpleStateCheckerContext<TState> context);
}
