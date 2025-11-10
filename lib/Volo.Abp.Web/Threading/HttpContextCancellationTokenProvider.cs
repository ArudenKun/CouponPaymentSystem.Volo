using System.Web;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Web.Threading;

public class HttpContextCancellationTokenProvider
    : CancellationTokenProviderBase,
        ITransientDependency
{
    private readonly ILogger<HttpContextCancellationTokenProvider> _logger;

    public HttpContextCancellationTokenProvider(
        IAmbientScopeProvider<CancellationTokenOverride> cancellationTokenOverrideScopeProvider,
        ILogger<HttpContextCancellationTokenProvider> logger
    )
        : base(cancellationTokenOverrideScopeProvider)
    {
        _logger = logger;
    }

    public override CancellationToken Token
    {
        get
        {
            if (OverrideValue != null)
            {
                return OverrideValue.CancellationToken;
            }

            try
            {
                return HttpContext.Current?.Response.ClientDisconnectedToken
                    ?? CancellationToken.None;
            }
            catch (HttpException ex)
            {
                /* Workaround:
                 * Accessing HttpContext.Response during Application_Start or Application_End will throw exception.
                 * This behavior is intentional from microsoft
                 * See https://stackoverflow.com/questions/2518057/request-is-not-available-in-this-context/23908099#comment2514887_2518066
                 */
                _logger.LogWarning(ex, "HttpContext.Request access when it is not suppose to");
                return CancellationToken.None;
            }
        }
    }
}
