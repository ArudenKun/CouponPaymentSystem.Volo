using Abp.Configuration.Startup;
using Abp.Runtime.Remoting;

namespace Abp.Runtime.Session;

/// <summary>
/// Implements null object pattern for <see cref="IAbpSession"/>.
/// </summary>
public class NullAbpSession : AbpSessionBase
{
    /// <summary>
    /// Singleton instance.
    /// </summary>
    public static NullAbpSession Instance { get; } = new NullAbpSession();

    /// <inheritdoc/>
    public override long? UserId => null;

    /// <inheritdoc/>
    public override Guid? TenantId => null;

    public override MultiTenancySides MultiTenancySide => MultiTenancySides.Tenant;

    public override long? ImpersonatorUserId => null;

    public override Guid? ImpersonatorTenantId => null;

    private NullAbpSession()
        : base(
            new MultiTenancyConfig(),
            new DataContextAmbientScopeProvider<SessionOverride>(new AsyncLocalAmbientDataContext())
        ) { }
}
