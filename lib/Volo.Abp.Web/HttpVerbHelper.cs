namespace Volo.Abp.Web;

public static class HttpVerbHelper
{
    public static HttpVerb Create(string httpMethod) =>
        httpMethod.ToUpperInvariant() switch
        {
            "GET" => HttpVerb.Get,
            "POST" => HttpVerb.Post,
            "PUT" => HttpVerb.Put,
            "DELETE" => HttpVerb.Delete,
            "OPTIONS" => HttpVerb.Options,
            "TRACE" => HttpVerb.Trace,
            "HEAD" => HttpVerb.Head,
            "PATCH" => HttpVerb.Patch,
            _ => throw new AbpException("Unknown HTTP METHOD: " + httpMethod),
        };
}
