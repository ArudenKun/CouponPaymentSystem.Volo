using System.Security.Claims;

namespace Abp.Runtime.Session;

public class DefaultPrincipalAccessor : IPrincipalAccessor, ISingletonDependency
{
    public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;

    public static DefaultPrincipalAccessor Instance => new DefaultPrincipalAccessor();
}
