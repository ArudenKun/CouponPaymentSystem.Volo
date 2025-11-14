using JetBrains.Annotations;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Web.MultiTenancy;
using Volo.Abp.Web.Resources;

namespace Volo.Abp.Web;

[PublicAPI]
[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpSecurityModule),
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpUnitOfWorkModule),
    typeof(AbpHttpModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpValidationModule),
    typeof(AbpExceptionHandlingModule)
)]
public class AbpWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context) { }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpTenantResolveOptions>(options =>
        {
            options.TenantResolvers.Add(new HttpCookieTenantResolveContributor());
            options.TenantResolvers.Add(new HttpCookieTenantResolveContributor());
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpWebModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources.Add<AbpWebResource>("en");
        });
    }
}
