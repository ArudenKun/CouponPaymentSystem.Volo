using System.Reflection;
using System.Web.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Web.Mvc.Controllers;

public class ControllerConventionalRegistrar : ConventionalRegistrarBase
{
    public override void AddType(IServiceCollection services, Type type)
    {
        if (!AbpTypeExtensions.IsAssignableTo(type, typeof(Controller)))
            return;

        if (!type.GetTypeInfo().IsGenericTypeDefinition)
        {
            services.AddTransient(type);
        }
    }
}
