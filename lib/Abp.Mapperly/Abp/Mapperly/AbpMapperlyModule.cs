using Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Mapperly;

[DependsOn(typeof(AbpCoreModule))]
public class AbpMapperlyModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddConventionalRegistrar(new AbpMapperlyConventionalRegistrar());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMapperlyObjectMapper();
    }
}
