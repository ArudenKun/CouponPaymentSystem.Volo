using Microsoft.Extensions.DependencyInjection;

namespace Castle.Windsor.MsDependencyInjection;

/// <summary>
/// Implements <see cref="IServiceScopeFactory"/>.
/// </summary>
public class WindsorServiceScopeFactory : IServiceScopeFactory
{
    private readonly IWindsorContainer _container;
    private readonly IMsLifetimeScope? _msLifetimeScope;

    public WindsorServiceScopeFactory(
        IWindsorContainer container,
        MsLifetimeScopeProvider msLifetimeScopeProvider
    )
    {
        _container = container;
        _msLifetimeScope = msLifetimeScopeProvider.LifetimeScope;
    }

    public IServiceScope CreateScope()
    {
        if (_msLifetimeScope is null)
        {
            throw new InvalidOperationException("MsLifetimeScope is null");
        }

        return new WindsorServiceScope(_container, _msLifetimeScope);
    }
}
