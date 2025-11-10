using System.Web;
using Volo.Abp.Modularity;

namespace Volo.Abp.Web;

public abstract class AbpWebApplication<TStartupModule> : HttpApplication
    where TStartupModule : IAbpModule
{
    /// <summary>
    /// Gets a reference to the <see cref="IAbpApplication"/> instance.
    /// </summary>
    public static IAbpApplicationWithInternalServiceProvider AbpApplication { get; } =
        AbpApplicationFactory.Create<TStartupModule>(options =>
        {
            options.UseAutofac();
        });

    protected virtual void Application_Start(object sender, EventArgs e)
    {
        AbpApplication.Initialize();
    }

    protected virtual void Application_End(object sender, EventArgs e)
    {
        AbpApplication.Shutdown();
        AbpApplication.Dispose();
    }

    protected virtual void Session_Start(object sender, EventArgs e) { }

    protected virtual void Session_End(object sender, EventArgs e) { }

    protected virtual void Application_BeginRequest(object sender, EventArgs e) { }

    protected virtual void Application_AuthenticateRequest(object sender, EventArgs e) { }

    protected virtual void Application_PostAuthenticateRequest(object sender, EventArgs e)
    {
        SetCurrentCulture();
    }

    protected virtual void Application_EndRequest(object sender, EventArgs e) { }

    protected virtual void Application_Error(object sender, EventArgs e) { }

    protected virtual void SetCurrentCulture()
    {
        // AbpBootstrapper.IocManager.Using<ICurrentCultureSetter>(cultureSetter =>
        //     cultureSetter.SetCurrentCulture(Context)
        // );
    }
}
