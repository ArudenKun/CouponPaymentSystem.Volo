using System.Net.Http;

namespace Volo.Abp.Web.Security;

public class AbpWebAntiForgeryOptions
{
    public bool IsEnabled { get; set; }

    public HashSet<HttpMethod> IgnoredHttpMethods { get; }

    public AbpWebAntiForgeryOptions()
    {
        IsEnabled = true;
        IgnoredHttpMethods =
        [
            HttpMethod.Get,
            HttpMethod.Head,
            HttpMethod.Options,
            HttpMethod.Trace,
        ];
    }
}
