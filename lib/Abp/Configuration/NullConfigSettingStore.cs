using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Abp.Logging;
using Abp.Threading;
using Microsoft.Extensions.Logging;

namespace Abp.Configuration
{
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

        private NullConfigSettingStore() { }

        public Task<SettingInfo?> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
            return Task.FromResult<SettingInfo?>(null);
        }

        public SettingInfo? GetSettingOrNull(int? tenantId, long? userId, string name)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
            return null;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(SettingInfo setting)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
            return AbpTaskCache.CompletedTask;
        }

        /// <inheritdoc/>
        public void Delete(SettingInfo setting)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
        }

        /// <inheritdoc/>
        public Task CreateAsync(SettingInfo setting)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
            return AbpTaskCache.CompletedTask;
        }

        /// <inheritdoc/>
        public void Create(SettingInfo setting)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
        }

        /// <inheritdoc/>
        public Task UpdateAsync(SettingInfo setting)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
            return AbpTaskCache.CompletedTask;
        }

        /// <inheritdoc/>
        public void Update(SettingInfo setting)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
        }

        /// <inheritdoc/>
        public Task<List<SettingInfo>> GetAllListAsync(int? tenantId, long? userId)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
            return Task.FromResult(new List<SettingInfo>());
        }

        /// <inheritdoc/>
        public List<SettingInfo> GetAllList(int? tenantId, long? userId)
        {
            LogHelper.Logger.LogWarning("ISettingStore is not implemented");
            return new List<SettingInfo>();
        }
    }
}
