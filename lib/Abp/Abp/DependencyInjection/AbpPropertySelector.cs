using System.Reflection;
using Autofac.Core;

namespace Abp.DependencyInjection;

public class AbpPropertySelector : DefaultPropertySelector
{
    public AbpPropertySelector(bool preserveSetValues)
        : base(preserveSetValues) { }

    public override bool InjectProperty(PropertyInfo propertyInfo, object instance)
    {
        return propertyInfo
                .GetCustomAttributes(typeof(DisablePropertyInjectionAttribute), true)
                .IsNullOrEmpty() && base.InjectProperty(propertyInfo, instance);
    }
}
