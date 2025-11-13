using Abp.Configuration.Startup;
using Abp.DependencyInjection;
using Abp.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Abp.MultiTenancy;

internal class TenantResolver : ITenantResolver, ITransientDependency
{
    private const string AmbientScopeContextKey = "Abp.MultiTenancy.TenantResolver.Resolving";

    private readonly IMultiTenancyConfig _multiTenancy;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ITenantStore _tenantStore;
    private readonly ITenantResolverCache _cache;
    private readonly IAmbientScopeProvider<bool> _ambientScopeProvider;

    public TenantResolver(
        IMultiTenancyConfig multiTenancy,
        IServiceScopeFactory serviceScopeFactory,
        ITenantStore tenantStore,
        ITenantResolverCache cache,
        IAmbientScopeProvider<bool> ambientScopeProvider,
        ILogger<TenantResolver> logger
    )
    {
        _multiTenancy = multiTenancy;
        _serviceScopeFactory = serviceScopeFactory;
        _tenantStore = tenantStore;
        _cache = cache;
        _ambientScopeProvider = ambientScopeProvider;
        Logger = logger;
    }

    public ILogger<TenantResolver> Logger { get; }

    public int? ResolveTenantId()
    {
        if (!_multiTenancy.Resolvers.Any())
        {
            return null;
        }

        if (_ambientScopeProvider.GetValue(AmbientScopeContextKey))
        {
            //Preventing recursive call of ResolveTenantId
            return null;
        }

        using (_ambientScopeProvider.BeginScope(AmbientScopeContextKey, true))
        {
            var cacheItem = _cache.Value;
            if (cacheItem != null)
            {
                return cacheItem.TenantId;
            }

            var tenantId = GetTenantIdFromContributors();
            _cache.Value = new TenantResolverCacheItem(tenantId);
            return tenantId;
        }
    }

    public Task<int?> ResolveTenantIdAsync()
    {
        return Task.FromResult(ResolveTenantId());
    }

    private int? GetTenantIdFromContributors()
    {
        foreach (var resolverType in _multiTenancy.Resolvers)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var resolver = (ITenantResolveContributor)
                scope.ServiceProvider.GetRequiredService(resolverType);
            int? tenantId;

            try
            {
                tenantId = resolver.ResolveTenantId();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, LogLevel.Warning);
                continue;
            }

            if (tenantId == null)
            {
                continue;
            }

            if (_tenantStore.Find(tenantId.Value) == null)
            {
                continue;
            }

            return tenantId;
        }

        return null;
    }
}
