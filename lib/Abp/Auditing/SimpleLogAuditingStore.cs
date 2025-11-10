using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Abp.Auditing;

/// <summary>
/// Implements <see cref="IAuditingStore"/> to simply write audits to logs.
/// </summary>
public class SimpleLogAuditingStore : IAuditingStore
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static SimpleLogAuditingStore Instance { get; } = new();

    public ILogger<SimpleLogAuditingStore> Logger { get; set; }

    public SimpleLogAuditingStore()
    {
        Logger = NullLogger<SimpleLogAuditingStore>.Instance;
    }

    public Task SaveAsync(AuditInfo auditInfo)
    {
        if (auditInfo.Exception == null)
        {
            Logger.LogInformation(auditInfo.ToString());
        }
        else
        {
            Logger.LogWarning(auditInfo.ToString());
        }

        return Task.FromResult(0);
    }

    public void Save(AuditInfo auditInfo)
    {
        if (auditInfo.Exception == null)
        {
            Logger.LogInformation(auditInfo.ToString());
        }
        else
        {
            Logger.LogWarning(auditInfo.ToString());
        }
    }
}
