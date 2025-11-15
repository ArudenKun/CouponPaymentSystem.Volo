using System.Web.Hosting;
using System.Web.Mvc;
using Abp.HangFire;
using Abp.HangFire.Configuration;
using Abp.Owin;
using Abp.Web;
using Hangfire;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Presentation;

[assembly: OwinStartup(typeof(MvcApplication), nameof(MvcApplication.Configuration))]

namespace Presentation;

public class MvcApplication : AbpWebApplication<PresentationModule>
{
    public void Configuration(IAppBuilder app)
    {
        app.UseAbp();
        app.UseCookieAuthentication(
            new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/authentication/sign-in"),
                CookieName = CookieAuthenticationDefaults.CookiePrefix + "CouponPaymentSystem",
            }
        );
        app.MapSignalR();
        app.UseHangfireAspNet(() =>
            [AbpBootstrapper.IocManager.Resolve<IAbpHangfireConfiguration>().Server]
        );
        app.UseHangfireDashboard(
            "/hangfire",
            new DashboardOptions
            {
                Authorization = [new AbpHangfireAuthorizationFilter()],
                AsyncAuthorization = [new AbpHangfireAsyncAuthorizationFilter()],
                AppPath = HostingEnvironment.ApplicationVirtualPath,
            }
        );
    }

    protected void Application_Start()
    {
        AreaRegistration.RegisterAllAreas();
    }
}
