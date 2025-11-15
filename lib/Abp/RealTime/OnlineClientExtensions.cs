using JetBrains.Annotations;

namespace Abp.RealTime;

public static class OnlineClientExtensions
{
    public static UserIdentifier? ToUserIdentifierOrNull(this IOnlineClient onlineClient)
    {
        return onlineClient.UserId.HasValue
            ? new UserIdentifier(onlineClient.TenantId, onlineClient.UserId.Value)
            : null;
    }
}
