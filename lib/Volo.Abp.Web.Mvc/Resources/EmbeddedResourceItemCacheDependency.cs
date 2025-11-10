using System.Web.Caching;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.Web.Mvc.Resources;

public class EmbeddedResourceItemCacheDependency : CacheDependency
{
    public EmbeddedResourceItemCacheDependency(IFileInfo fileInfo)
    {
        SetUtcLastModified(fileInfo.LastModified.DateTime);
    }

    public EmbeddedResourceItemCacheDependency()
    {
        SetUtcLastModified(DateTime.UtcNow);
    }
}
