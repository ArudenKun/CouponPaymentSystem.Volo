using Abp.Collections;

namespace Abp.SimpleStateChecking;

public class AbpSimpleStateCheckerOptions<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public ITypeList<ISimpleStateChecker<TState>> GlobalStateCheckers { get; }

    public AbpSimpleStateCheckerOptions()
    {
        GlobalStateCheckers = new TypeList<ISimpleStateChecker<TState>>();
    }
}
