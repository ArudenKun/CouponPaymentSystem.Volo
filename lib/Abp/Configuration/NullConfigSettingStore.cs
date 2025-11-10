using Abp.Dependency;
using Abp.Threading;
using Microsoft.Extensions.Logging;

namespace Abp.Configuration;

/// <summary>
/// Implements default behavior for ISettingStore.
/// Only <see cref="GetSettingOrNullAsync"/> method is implemented and it gets setting's value
/// from application's configuration file if exists, or returns null if not.
/// </summary>
public class NullConfigSettingStore : ISettingStore
{
    /// <summary>
    /// Gets singleton instance.
    /// </summary>
    public static NullConfigSettingStore Instance { get; } = new NullConfigSettingStore();

    public static ILogger<NullConfigSettingStore> Logger =>
        IocManager.Instance.Resolve<ILogger<NullConfigSettingStore>>();

    private NullConfigSettingStore() { }

    public Task<SettingInfo?> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
    {
        return Task.FromResult<SettingInfo?>(null);
    }

    public SettingInfo? GetSettingOrNull(int? tenantId, long? userId, string name)
    {
        return null;
    }

    /// <inheritdoc/>
    public Task DeleteAsync(SettingInfo setting)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support DeleteAsync."
        );
        return AbpTaskCache.CompletedTask;
    }

    /// <inheritdoc/>
    public void Delete(SettingInfo setting)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support DeleteAsync."
        );
    }

    /// <inheritdoc/>
    public Task CreateAsync(SettingInfo setting)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support CreateAsync."
        );
        return AbpTaskCache.CompletedTask;
    }

    /// <inheritdoc/>
    public void Create(SettingInfo setting)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support CreateAsync."
        );
    }

    /// <inheritdoc/>
    public Task UpdateAsync(SettingInfo setting)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support UpdateAsync."
        );
        return AbpTaskCache.CompletedTask;
    }

    /// <inheritdoc/>
    public void Update(SettingInfo setting)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support UpdateAsync."
        );
    }

    /// <inheritdoc/>
    public Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support GetAllListAsync."
        );
        return Task.FromResult(new List<SettingInfo>());
    }

    /// <inheritdoc/>
    public List<SettingInfo> GetAllList(int? tenantId, long? userId)
    {
        Logger.LogWarning(
            "ISettingStore is not implemented, using DefaultConfigSettingStore which does not support GetAllListAsync."
        );
        return new List<SettingInfo>();
    }
}
