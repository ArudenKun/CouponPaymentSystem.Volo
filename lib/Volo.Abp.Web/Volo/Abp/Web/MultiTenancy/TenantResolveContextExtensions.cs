using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Web.MultiTenancy;

public static class TenantResolveContextExtensions
{
    extension(ITenantResolveContext context)
    {
        public AbpWebMultiTenancyOptions GetAbpWebMultiTenancyOptions()
        {
            return context
                .ServiceProvider.GetRequiredService<IOptions<AbpWebMultiTenancyOptions>>()
                .Value;
        }
    }
}
