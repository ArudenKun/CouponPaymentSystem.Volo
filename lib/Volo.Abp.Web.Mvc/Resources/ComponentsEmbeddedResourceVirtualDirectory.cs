using System.Collections;
using System.Web.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Volo.Abp.Web.Mvc.Resources;

public class ComponentsEmbeddedResourceVirtualDirectory : VirtualDirectory
{
    private readonly List<EmbeddedResourceVirtualFile> _children = [];
    private readonly List<EmbeddedResourceVirtualFile> _files = [];

    public ComponentsEmbeddedResourceVirtualDirectory(
        string virtualPath,
        IEnumerable<IFileInfo> embeddedResourceItems
    )
        : base(virtualPath)
    {
        var virtualPathPart = string.Copy(virtualPath).TrimStart('/', '~').Replace('/', '.');
        var realPathPart = string.Copy(virtualPath).TrimStart(new[] { '/', '~' });

        foreach (var embeddedResourceItem in embeddedResourceItems)
        {
            var fileVirtualPath = embeddedResourceItem
                .GetVirtualOrPhysicalPathOrNull()
                ?.Replace(virtualPathPart, string.Empty)
                .TrimStart('.');

            var virtualFile = new EmbeddedResourceVirtualFile(
                $"/{realPathPart}/{fileVirtualPath}",
                embeddedResourceItem
            );

            _files.Add(virtualFile);
            _children.Add(virtualFile);
        }
    }

    public override IEnumerable Directories => new List<VirtualDirectory>();

    public override IEnumerable Files => _files;
    public override IEnumerable Children => _children;
}
