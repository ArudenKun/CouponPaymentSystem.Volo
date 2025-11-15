using Abp.Dependency;
using Castle.MicroKernel.Registration;

namespace Abp.Mapperly;

internal class AbpMapperlyConventionalRegistrar : IConventionalDependencyRegistrar
{
    public void RegisterAssembly(IConventionalRegistrationContext context)
    {
        context.IocManager.Register(
            Classes
                .FromAssembly(context.Assembly)
                .IncludeNonPublicTypes()
                .BasedOn(typeof(IMapperlyMapper<,>))
                .WithService.Self()
                .WithService.DefaultInterfaces()
                .LifestyleTransient()
        );

        context.IocManager.Register(
            Classes
                .FromAssembly(context.Assembly)
                .IncludeNonPublicTypes()
                .BasedOn(typeof(IReverseMapperlyMapper<,>))
                .WithService.Self()
                .WithService.DefaultInterfaces()
                .LifestyleTransient()
        );
    }
}
