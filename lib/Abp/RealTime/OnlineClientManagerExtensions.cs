using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Abp.RealTime;

/// <summary>
/// Extension methods for <see cref="IOnlineClientManager"/>.
/// </summary>
public static class OnlineClientManagerExtensions
{
    /// <summary>
    /// Determines whether the specified user is online or not.
    /// </summary>
    /// <param name="onlineClientManager">The online client manager.</param>
    /// <param name="user">User.</param>
    public static async Task<bool> IsOnlineAsync(
        this IOnlineClientManager onlineClientManager,
        UserIdentifier user
    )
    {
        return (await onlineClientManager.GetAllByUserIdAsync(user)).Any();
    }

    public static async Task<bool> RemoveAsync(
        this IOnlineClientManager onlineClientManager,
        IOnlineClient client
    )
    {
        Check.NotNull(onlineClientManager, nameof(onlineClientManager));
        Check.NotNull(client, nameof(client));

        return await onlineClientManager.RemoveAsync(client.ConnectionId);
    }
}
