using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Web.AntiForgery;

internal class AbpAntiForgeryManager
    : IAbpAntiForgeryManager,
        IAbpAntiForgeryValidator,
        ITransientDependency
{
    public AbpAntiForgeryOptions Options { get; }

    public AbpAntiForgeryManager(IOptions<AbpAntiForgeryOptions> options)
    {
        Options = options.Value;
    }

    public virtual string GenerateToken()
    {
        return GuidPolyfill.CreateVersion7().ToString("D");
    }

    public virtual bool IsValid(string cookieValue, string tokenValue)
    {
        return cookieValue == tokenValue;
    }
}
