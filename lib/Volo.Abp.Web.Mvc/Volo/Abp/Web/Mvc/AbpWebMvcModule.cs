using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;
using Volo.Abp.Web.Mvc.Security;
using Volo.Abp.Web.Security;

namespace Volo.Abp.Web.Mvc;

[DependsOn(typeof(AbpWebModule))]
public class AbpWebMvcModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Replace(
            ServiceDescriptor.Singleton<IAbpAntiForgeryManager, AbpMvcAntiForgeryManager>()
        );
    }

    public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
    {
        // GlobalFilters.Filters.Add(context.ServiceProvider.GetRequiredService<>());
    }
}
