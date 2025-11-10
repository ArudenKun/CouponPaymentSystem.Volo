using System.Web.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.Web.Mvc.Resources;

public class EmbeddedResourceItemVirtualFile : VirtualFile
{
    private readonly IFileInfo _fileInfo;

    public EmbeddedResourceItemVirtualFile(string virtualPath, IFileInfo fileInfo)
        : base(virtualPath)
    {
        _fileInfo = fileInfo;
    }

    public override Stream Open()
    {
        return _fileInfo.CreateReadStream();
    }
}
