using Microsoft.Owin.FileSystems;
using Volo.Abp.DependencyInjection;
using Volo.Abp.VirtualFileSystem;
using IOwinFileInfo = Microsoft.Owin.FileSystems.IFileInfo;

namespace Volo.Abp.Web.Owin;

public class OwinEmbeddedResourceFileSystem : IFileSystem, ITransientDependency
{
    private readonly IVirtualFileProvider _virtualFileProvider;
    private readonly IFileSystem _physicalFileSystem;

    public OwinEmbeddedResourceFileSystem(
        IVirtualFileProvider virtualFileProvider,
        string rootFolder
    )
    {
        _virtualFileProvider = virtualFileProvider;
        _physicalFileSystem = new PhysicalFileSystem(rootFolder);
    }

    public bool TryGetFileInfo(string subpath, out IOwinFileInfo? content)
    {
        if (_physicalFileSystem.TryGetFileInfo(subpath, out content))
        {
            return true;
        }

        var virtualContent = _virtualFileProvider.GetFileInfo(subpath);
        if (!virtualContent.Exists)
        {
            content = null;
            return false;
        }
        content = new OwinEmbeddedResourceFileInfo(virtualContent);
        return true;
    }

    public bool TryGetDirectoryContents(string subpath, out IEnumerable<IOwinFileInfo> contents)
    {
        if (_physicalFileSystem.TryGetDirectoryContents(subpath, out contents))
        {
            return true;
        }

        var virtualContents = _virtualFileProvider.GetDirectoryContents(subpath);
        contents = virtualContents
            .Where(x => x.Exists)
            .Select(x => new OwinEmbeddedResourceFileInfo(x));
        return false;
    }
}
