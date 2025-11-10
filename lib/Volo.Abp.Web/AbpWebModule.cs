using System.Web;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.Autofac;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Web.MultiTenancy;

namespace Volo.Abp.Web;

[DependsOn(
    typeof(AbpAutofacModule),
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
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var ignoredTypes = new[]
        {
            typeof(HttpPostedFileBase),
            typeof(IEnumerable<HttpPostedFileBase>),
            typeof(HttpPostedFileWrapper),
            typeof(IEnumerable<HttpPostedFileWrapper>),
        };

        Configure<AbpTenantResolveOptions>(options =>
        {
            options.AddDomainTenantResolver("");
            options.TenantResolvers.Add(new HeaderTenantResolveContributor());
            options.TenantResolvers.Add(new CookieTenantResolveContributor());
        });

        Configure<AbpValidationOptions>(options =>
        {
            options.IgnoredTypes.AddIfNotContains(ignoredTypes);
        });
        Configure<AbpAuditingOptions>(options =>
        {
            options.IgnoredTypes.AddIfNotContains(ignoredTypes);
        });
    }
}
