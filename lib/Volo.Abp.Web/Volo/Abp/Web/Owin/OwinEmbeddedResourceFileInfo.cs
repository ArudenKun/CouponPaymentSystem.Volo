using IFileInfo = Microsoft.Extensions.FileProviders.IFileInfo;
using IOwinFileInfo = Microsoft.Owin.FileSystems.IFileInfo;

namespace Volo.Abp.Web.Owin;

public class OwinEmbeddedResourceFileInfo : IOwinFileInfo
{
    public long Length => _fileInfo.Length;

    public string? PhysicalPath => null;

    public string Name => _fileInfo.Name;

    public DateTime LastModified => _fileInfo.LastModified.UtcDateTime;

    public bool IsDirectory => false;

    private readonly IFileInfo _fileInfo;

    public OwinEmbeddedResourceFileInfo(IFileInfo fileInfo)
    {
        _fileInfo = fileInfo;
    }

    public Stream CreateReadStream() => _fileInfo.CreateReadStream();
}
