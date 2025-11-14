using Abp.Aspects;
using Abp.DependencyInjection;
using Abp.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Runtime.Validation.Interception;

/// <summary>
/// This interceptor is used intercept method calls for classes which's methods must be validated.
/// </summary>
public class ValidationInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        if (
            AbpCrossCuttingConcerns.IsApplied(
                invocation.TargetObject,
                AbpCrossCuttingConcerns.Validation
            )
        )
        {
            await invocation.ProceedAsync();
            return;
        }

        using (
            var validator =
                _serviceProvider.GetRequiredServiceAsDisposable<MethodInvocationValidator>()
        )
        {
            validator.Service.Initialize(invocation.Method, invocation.Arguments);
            validator.Service.Validate();
        }

        await invocation.ProceedAsync();
    }
}
