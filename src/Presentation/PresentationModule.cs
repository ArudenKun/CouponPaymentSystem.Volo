using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.WebApi;

namespace Presentation;

[DependsOn(typeof(AbpWebMvcModule), typeof(AbpWebApiModule), typeof(AbpWebSignalRModule))]
public class PresentationModule : AbpModule
{
    public override void PreInitialize()
    {
        base.PreInitialize();
    }
}
