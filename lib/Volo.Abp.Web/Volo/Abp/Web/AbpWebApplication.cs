using System.Web;
using Volo.Abp.Modularity;

namespace Volo.Abp.Web;

/// <summary>
/// This class is used to simplify starting of ABP system using <see cref="AbpApplication"/> class..
/// Inherit from this class in global.asax instead of <see cref="HttpApplication"/> to be able to start ABP system.
/// </summary>
/// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>.</typeparam>
public abstract class AbpWebApplication<TStartupModule> : HttpApplication
    where TStartupModule : AbpModule
{
    /// <summary>
    /// Gets a reference to the <see cref="AbpApplication"/> instance.
    /// </summary>
    public static IAbpApplication AbpApplication => InternalAbpApplication;

    private static readonly IAbpApplicationWithInternalServiceProvider InternalAbpApplication =
        AbpApplicationFactory.Create<TStartupModule>(options => options.UseAutofac());

    protected virtual void Application_Start(object sender, EventArgs e)
    {
        InternalAbpApplication.Initialize();
    }

    protected virtual void Application_End(object sender, EventArgs e)
    {
        InternalAbpApplication.Shutdown();
        InternalAbpApplication.Dispose();
    }

    protected virtual void Session_Start(object sender, EventArgs e) { }

    protected virtual void Session_End(object sender, EventArgs e) { }

    protected virtual void Application_BeginRequest(object sender, EventArgs e) { }

    protected virtual void Application_AuthenticateRequest(object sender, EventArgs e) { }

    protected virtual void Application_PostAuthenticateRequest(object sender, EventArgs e) { }

    protected virtual void Application_EndRequest(object sender, EventArgs e) { }

    protected virtual void Application_Error(object sender, EventArgs e) { }
}
