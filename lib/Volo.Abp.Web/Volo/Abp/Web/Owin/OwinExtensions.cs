using System.Web;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Owin.StaticFiles;
using Owin;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Web.Owin;

/// <summary>
/// OWIN extension methods for ABP.
/// </summary>
public static class OwinExtensions
{
    /// <param name="app">The application.</param>
    extension(IAppBuilder app)
    {
        /// <summary>
        /// This should be called as the first line for OWIN based applications for ABP framework.
        /// </summary>
        /// <param name="abpApplication">The Abp application</param>
        public void UseAbp(IAbpApplication abpApplication)
        {
            app.UseAutofacMiddleware(abpApplication.ServiceProvider.GetAutofacRoot());
            app.UseAbp(abpApplication, null);
        }

        public void UseAbp(IAbpApplication abpApplication, Action<AbpWebOwinOptions>? optionsAction)
        {
            var options = abpApplication
                .ServiceProvider.GetRequiredService<IOptions<AbpWebOwinOptions>>()
                .Value;
            options.UseEmbeddedFiles = HttpContext.Current?.Server != null;
            optionsAction?.Invoke(options);

            if (!options.UseEmbeddedFiles)
                return;

            if (HttpContext.Current?.Server == null)
            {
                throw new AbpInitializationException(
                    "Can not enable UseEmbeddedFiles for OWIN since HttpContext.Current is null! If you are using ASP.NET Core, serve embedded resources through ASP.NET Core middleware instead of OWIN. See http://www.aspnetboilerplate.com/Pages/Documents/Embedded-Resource-Files#aspnet-core-configuration"
                );
            }

            app.UseStaticFiles(
                new StaticFileOptions
                {
                    FileSystem = new OwinEmbeddedResourceFileSystem(
                        abpApplication.ServiceProvider.GetRequiredService<IVirtualFileProvider>(),
                        HttpContext.Current.Server.MapPath("~/")
                    ),
                }
            );
            options.UseAbpSet = true;
        }
    }
}
