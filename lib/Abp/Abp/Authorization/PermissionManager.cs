using System.Collections.Immutable;
using Abp.Application.Features;
using Abp.Configuration.Startup;
using Abp.DependencyInjection;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.Authorization;

/// <summary>
/// Permission manager.
/// </summary>
public class PermissionManager
    : PermissionDefinitionContextBase,
        IPermissionManager,
        ISingletonDependency
{
    public IAbpSession AbpSession { get; set; }

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IAuthorizationConfiguration _authorizationConfiguration;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IMultiTenancyConfig _multiTenancy;

    /// <summary>
    /// Constructor.
    /// </summary>
    public PermissionManager(
        IServiceScopeFactory serviceScopeFactory,
        IAuthorizationConfiguration authorizationConfiguration,
        IUnitOfWorkManager unitOfWorkManager,
        IMultiTenancyConfig multiTenancy
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _authorizationConfiguration = authorizationConfiguration;
        _unitOfWorkManager = unitOfWorkManager;
        _multiTenancy = multiTenancy;

        AbpSession = NullAbpSession.Instance;
    }

    public virtual void Initialize()
    {
        foreach (var providerType in _authorizationConfiguration.Providers)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var provider = (AuthorizationProvider)
                scope.ServiceProvider.GetRequiredService(providerType);
            provider.SetPermissions(this);
        }

        Permissions.AddAllPermissions();
    }

    public virtual Permission GetPermission(string name)
    {
        var permission = Permissions.GetOrDefault(name);
        if (permission == null)
        {
            throw new AbpException("There is no permission with name: " + name);
        }

        return permission;
    }

    public virtual IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var featureDependencyContext =
            scope.ServiceProvider.GetRequiredService<FeatureDependencyContext>();
        featureDependencyContext.TenantId = GetCurrentTenantId();

        return Permissions
            .Values.WhereIf(
                tenancyFilter,
                p => p.MultiTenancySides.HasFlag(GetCurrentMultiTenancySide())
            )
            .Where(p =>
                p.FeatureDependency == null
                || GetCurrentMultiTenancySide() == MultiTenancySides.Host
                || p.FeatureDependency.IsSatisfied(featureDependencyContext)
            )
            .ToImmutableList();
    }

    public virtual async Task<IReadOnlyList<Permission>> GetAllPermissionsAsync(
        bool tenancyFilter = true
    )
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var featureDependencyContext =
            scope.ServiceProvider.GetRequiredService<FeatureDependencyContext>();
        featureDependencyContext.TenantId = GetCurrentTenantId();

        var permissions = Permissions
            .Values.WhereIf(
                tenancyFilter,
                p => p.MultiTenancySides.HasFlag(GetCurrentMultiTenancySide())
            )
            .ToList();

        var result = await FilterSatisfiedPermissionsAsync(
            featureDependencyContext,
            permissions,
            p =>
                p.FeatureDependency == null
                || GetCurrentMultiTenancySide() == MultiTenancySides.Host
        );

        return result.ToImmutableList();
    }

    public virtual IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var featureDependencyContext =
            scope.ServiceProvider.GetRequiredService<FeatureDependencyContext>();
        featureDependencyContext.TenantId = GetCurrentTenantId();

        return Permissions
            .Values.Where(p => p.MultiTenancySides.HasFlag(multiTenancySides))
            .Where(p =>
                p.FeatureDependency == null
                || GetCurrentMultiTenancySide() == MultiTenancySides.Host
                || (
                    p.MultiTenancySides.HasFlag(MultiTenancySides.Host)
                    && multiTenancySides.HasFlag(MultiTenancySides.Host)
                )
                || p.FeatureDependency.IsSatisfied(featureDependencyContext)
            )
            .ToImmutableList();
    }

    public virtual async Task<IReadOnlyList<Permission>> GetAllPermissionsAsync(
        MultiTenancySides multiTenancySides
    )
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var featureDependencyContext =
            scope.ServiceProvider.GetRequiredService<FeatureDependencyContext>();
        featureDependencyContext.TenantId = GetCurrentTenantId();

        var permissions = Permissions
            .Values.Where(p => p.MultiTenancySides.HasFlag(multiTenancySides))
            .ToList();

        var result = await FilterSatisfiedPermissionsAsync(
            featureDependencyContext,
            permissions,
            p =>
                p.FeatureDependency == null
                || GetCurrentMultiTenancySide() == MultiTenancySides.Host
                || (
                    p.MultiTenancySides.HasFlag(MultiTenancySides.Host)
                    && multiTenancySides.HasFlag(MultiTenancySides.Host)
                )
        );

        return result.ToImmutableList();
    }

    private async Task<IList<Permission>> FilterSatisfiedPermissionsAsync(
        FeatureDependencyContext featureDependencyContextObject,
        IList<Permission> unfilteredPermissions,
        Func<Permission, bool> filter
    )
    {
        var filteredPermissions = new List<Permission>();

        foreach (var permission in unfilteredPermissions)
        {
            if (
                !filter.Invoke(permission)
                && permission.FeatureDependency is { } featureDependency
                && !await featureDependency.IsSatisfiedAsync(featureDependencyContextObject)
            )
            {
                continue;
            }

            filteredPermissions.Add(permission);
        }

        return filteredPermissions;
    }

    private MultiTenancySides GetCurrentMultiTenancySide()
    {
        if (_unitOfWorkManager.Current != null)
        {
            return _multiTenancy.IsEnabled && !_unitOfWorkManager.Current.GetTenantId().HasValue
                ? MultiTenancySides.Host
                : MultiTenancySides.Tenant;
        }

        return AbpSession.MultiTenancySide;
    }

    private Guid? GetCurrentTenantId()
    {
        if (_unitOfWorkManager.Current != null)
        {
            return _unitOfWorkManager.Current.GetTenantId();
        }

        return AbpSession.TenantId;
    }
}
