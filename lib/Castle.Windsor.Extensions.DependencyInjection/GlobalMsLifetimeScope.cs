namespace Castle.Windsor.Microsoft.DependencyInjection;

public class GlobalMsLifetimeScope : MsLifetimeScope
{
    public GlobalMsLifetimeScope(IWindsorContainer container)
        : base(container) { }

    protected override void DisposeInternal()
    {
        base.DisposeInternal();
        Container.Dispose();
    }
}
