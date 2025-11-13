using Abp.DependencyInjection;
using Abp.DynamicProxy;

namespace Abp.Authorization;

/// <summary>
/// This class is used to intercept methods to make authorization if the method defined <see cref="AbpAuthorizeAttribute"/>.
/// </summary>
internal class AuthorizationInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly IAuthorizationHelper _authorizationHelper;

    public AuthorizationInterceptor(IAuthorizationHelper authorizationHelper)
    {
        _authorizationHelper = authorizationHelper;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        var targetType = invocation.TargetObject?.GetType();
        if (targetType is null)
            return;

        await _authorizationHelper.AuthorizeAsync(invocation.Method, targetType);
        await invocation.ProceedAsync();
    }
}
