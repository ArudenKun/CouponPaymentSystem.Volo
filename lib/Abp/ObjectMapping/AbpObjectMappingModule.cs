namespace Abp.ObjectMapping;

// public class AbpObjectMappingModule : AbpModule
// {
//     public override void PreConfigureServices(ServiceConfigurationContext context)
//     {
//         context.Services.OnExposing(onServiceExposingContext =>
//         {
//             //Register types for IObjectMapper<TSource, TDestination> if implements
//             onServiceExposingContext.ExposedTypes.AddRange(
//                 ReflectionHelper
//                     .GetImplementedGenericTypes(
//                         onServiceExposingContext.ImplementationType,
//                         typeof(IObjectMapper<,>)
//                     )
//                     .ConvertAll(t => new ServiceIdentifier(t))
//             );
//         });
//     }
//
//     public override void ConfigureServices(ServiceConfigurationContext context)
//     {
//         context.Services.AddTransient(typeof(IObjectMapper<>), typeof(DefaultObjectMapper<>));
//     }
// }
