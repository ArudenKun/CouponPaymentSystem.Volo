using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Owin;
using Volo.Abp.Web.Owin;

namespace Volo.Abp.Web.Mvc.Owin;

public static class OwinExtensions
{
    public static IAppBuilder UseAbpMvc(this IAppBuilder app, IAbpApplication abpApplication)
    {
        var owinOptions = abpApplication
            .ServiceProvider.GetRequiredService<IOptions<AbpWebOwinOptions>>()
            .Value;
        if (!owinOptions.UseAbpSet)
            app.UseAutofacMiddleware(abpApplication.ServiceProvider.GetAutofacRoot());
        app.UseAutofacMvc();
        return app;
    }
}
