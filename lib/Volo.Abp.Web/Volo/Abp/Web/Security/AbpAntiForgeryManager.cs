using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Web.Security;

public class AbpAntiForgeryManager
    : IAbpAntiForgeryManager,
        IAbpAntiForgeryValidator,
        ITransientDependency
{
    public AbpAntiForgeryManager(IOptions<AbpAntiForgeryOptions> options)
    {
        Options = options.Value;
    }

    public AbpAntiForgeryOptions Options { get; }

    public virtual string GenerateToken()
    {
        return Guid.NewGuid().ToString("D");
    }

    public virtual bool IsValid(string cookieValue, string tokenValue)
    {
        return cookieValue == tokenValue;
    }
}
