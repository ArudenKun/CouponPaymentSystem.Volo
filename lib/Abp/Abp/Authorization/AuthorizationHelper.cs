using System.Reflection;
using Abp.Application.Features;
using Abp.Configuration.Startup;
using Abp.DependencyInjection;
using Abp.Localization;
using Abp.Reflection;
using Abp.Runtime.Session;

namespace Abp.Authorization;

internal class AuthorizationHelper : IAuthorizationHelper, ITransientDependency
{
    private readonly IFeatureChecker _featureChecker;
    private readonly IAuthorizationConfiguration _authConfiguration;
    private readonly IAbpSession _abpSession;
    private readonly IPermissionChecker _permissionChecker;
    private readonly ILocalizationManager _localizationManager;

    public AuthorizationHelper(
        IFeatureChecker featureChecker,
        IAuthorizationConfiguration authConfiguration,
        IAbpSession abpSession,
        IPermissionChecker permissionChecker,
        ILocalizationManager localizationManager
    )
    {
        _featureChecker = featureChecker;
        _authConfiguration = authConfiguration;
        _abpSession = abpSession;
        _permissionChecker = permissionChecker;
        _localizationManager = localizationManager;
    }

    public virtual async Task AuthorizeAsync(
        IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes
    )
    {
        if (!_authConfiguration.IsEnabled)
        {
            return;
        }

        if (!_abpSession.UserId.HasValue)
        {
            throw new AbpAuthorizationException(
                _localizationManager.GetString(
                    AbpConsts.LocalizationSourceName,
                    "CurrentUserDidNotLoginToTheApplication"
                )
            );
        }

        foreach (var authorizeAttribute in authorizeAttributes)
        {
            await _permissionChecker.AuthorizeAsync(
                authorizeAttribute.RequireAllPermissions,
                authorizeAttribute.Permissions
            );
        }
    }

    public virtual void Authorize(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
    {
        if (!_authConfiguration.IsEnabled)
        {
            return;
        }

        if (!_abpSession.UserId.HasValue)
        {
            throw new AbpAuthorizationException(
                _localizationManager.GetString(
                    AbpConsts.LocalizationSourceName,
                    "CurrentUserDidNotLoginToTheApplication"
                )
            );
        }

        foreach (var authorizeAttribute in authorizeAttributes)
        {
            _permissionChecker.Authorize(
                authorizeAttribute.RequireAllPermissions,
                authorizeAttribute.Permissions
            );
        }
    }

    public virtual async Task AuthorizeAsync(MethodInfo methodInfo, Type type)
    {
        await CheckFeaturesAsync(methodInfo, type);
        await CheckPermissionsAsync(methodInfo, type);
    }

    public virtual void Authorize(MethodInfo methodInfo, Type type)
    {
        CheckFeatures(methodInfo, type);
        CheckPermissions(methodInfo, type);
    }

    protected virtual async Task CheckFeaturesAsync(MethodInfo methodInfo, Type type)
    {
        var featureAttributes =
            ReflectionHelper.GetAttributesOfMemberAndType<RequiresFeatureAttribute>(
                methodInfo,
                type
            );

        if (featureAttributes.Count <= 0)
        {
            return;
        }

        foreach (var featureAttribute in featureAttributes)
        {
            await _featureChecker.CheckEnabledAsync(
                featureAttribute.RequiresAll,
                featureAttribute.Features
            );
        }
    }

    protected virtual void CheckFeatures(MethodInfo methodInfo, Type type)
    {
        var featureAttributes =
            ReflectionHelper.GetAttributesOfMemberAndType<RequiresFeatureAttribute>(
                methodInfo,
                type
            );

        if (featureAttributes.Count <= 0)
        {
            return;
        }

        foreach (var featureAttribute in featureAttributes)
        {
            _featureChecker.CheckEnabled(featureAttribute.RequiresAll, featureAttribute.Features);
        }
    }

    protected virtual async Task CheckPermissionsAsync(MethodInfo methodInfo, Type type)
    {
        if (!_authConfiguration.IsEnabled)
        {
            return;
        }

        if (AllowAnonymous(methodInfo, type))
        {
            return;
        }

        if (ReflectionHelper.IsPropertyGetterSetterMethod(methodInfo, type))
        {
            return;
        }

        if (
            !methodInfo.IsPublic
            && !methodInfo.GetCustomAttributes().OfType<IAbpAuthorizeAttribute>().Any()
        )
        {
            return;
        }

        var authorizeAttributes = ReflectionHelper
            .GetAttributesOfMemberAndType(methodInfo, type)
            .OfType<IAbpAuthorizeAttribute>()
            .ToArray();

        if (!authorizeAttributes.Any())
        {
            return;
        }

        await AuthorizeAsync(authorizeAttributes);
    }

    protected virtual void CheckPermissions(MethodInfo methodInfo, Type type)
    {
        if (!_authConfiguration.IsEnabled)
        {
            return;
        }

        if (AllowAnonymous(methodInfo, type))
        {
            return;
        }

        if (ReflectionHelper.IsPropertyGetterSetterMethod(methodInfo, type))
        {
            return;
        }

        if (
            !methodInfo.IsPublic
            && !methodInfo.GetCustomAttributes().OfType<IAbpAuthorizeAttribute>().Any()
        )
        {
            return;
        }

        var authorizeAttributes = ReflectionHelper
            .GetAttributesOfMemberAndType(methodInfo, type)
            .OfType<IAbpAuthorizeAttribute>()
            .ToArray();

        if (!authorizeAttributes.Any())
        {
            return;
        }

        Authorize(authorizeAttributes);
    }

    private static bool AllowAnonymous(MemberInfo memberInfo, Type type)
    {
        return ReflectionHelper
            .GetAttributesOfMemberAndType(memberInfo, type)
            .OfType<IAbpAllowAnonymousAttribute>()
            .Any();
    }
}
