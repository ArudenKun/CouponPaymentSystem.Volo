namespace Volo.Abp.Web.AntiForgery;

public class AbpAntiForgeryWebOptions
{
    public bool IsEnabled { get; set; } = true;

    public HashSet<HttpVerb> IgnoredHttpVerbs { get; } =
    [HttpVerb.Get, HttpVerb.Head, HttpVerb.Options, HttpVerb.Trace];
}
