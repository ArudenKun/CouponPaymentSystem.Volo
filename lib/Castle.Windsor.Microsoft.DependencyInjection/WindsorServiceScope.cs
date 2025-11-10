using Microsoft.Extensions.DependencyInjection;

namespace Castle.Windsor.Microsoft.DependencyInjection;

/// <summary>
/// Implements <see cref="IServiceScope"/>.
/// </summary>
public class WindsorServiceScope : IServiceScope
{
    public IServiceProvider ServiceProvider { get; }

    public MsLifetimeScope LifetimeScope { get; }

    private readonly IWindsorContainer _container;
    private readonly IMsLifetimeScope _parentLifetimeScope;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public WindsorServiceScope(IWindsorContainer container, IMsLifetimeScope currentMsLifetimeScope)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
        _container = container;
        _parentLifetimeScope = currentMsLifetimeScope;

        LifetimeScope = new MsLifetimeScope(container);

        _parentLifetimeScope?.AddChild(LifetimeScope);

        using (MsLifetimeScope.Using(LifetimeScope))
        {
            ServiceProvider = container.Resolve<IServiceProvider>();
        }
    }

    public void Dispose()
    {
        _parentLifetimeScope?.RemoveChild(LifetimeScope);
        LifetimeScope.Dispose();
        _container.Release(ServiceProvider);
    }
}
