using Abp.Dependency;
using Abp.Modules;
using Abp.ObjectMapping;

namespace Abp.Mapperly;

[DependsOn(typeof(AbpKernelModule))]
public class AbpMapperlyModule : AbpModule
{
    public override void PreInitialize()
    {
        IocManager.AddConventionalRegistrar(new AbpMapperlyConventionalRegistrar());
    }

    public override void Initialize()
    {
        IocManager.Register<IAutoObjectMappingProvider, MapperlyAutoObjectMappingProvider>(
            DependencyLifeStyle.Transient
        );
    }
}
