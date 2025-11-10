using System.Web.Mvc;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Web.Mvc.Controllers;
using Volo.Abp.Web.Mvc.Uow;
using Volo.Abp.Web.Mvc.Validation;

namespace Volo.Abp.Web.Mvc;

[DependsOn(typeof(AbpWebModule), typeof(AbpVirtualFileSystemModule))]
public class AbpWebMvcModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddConventionalRegistrar(new ControllerConventionalRegistrar());
        var containerBuilder = context.Services.GetContainerBuilder();
        containerBuilder.RegisterFilterProvider();
        containerBuilder.RegisterModelBinderProvider();
        containerBuilder.RegisterModule<AutofacWebTypesModule>();
        containerBuilder.RegisterSource(new ViewRegistrationSource());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWebMvcModule>();
        });

        base.ConfigureServices(context);
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        // GlobalFilters.Filters.Add(IocManager.Resolve<AbpMvcAuthorizeFilter>());
        // GlobalFilters.Filters.Add(IocManager.Resolve<AbpAntiForgeryMvcFilter>());
        // GlobalFilters.Filters.Add(IocManager.Resolve<AbpMvcAuditFilter>());

        base.PostConfigureServices(context);
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        GlobalFilters.Filters.Add(
            context.ServiceProvider.GetRequiredService<AbpMvcValidationFilter>()
        );
        GlobalFilters.Filters.Add(context.ServiceProvider.GetRequiredService<AbpMvcUowFilter>());
    }
}
