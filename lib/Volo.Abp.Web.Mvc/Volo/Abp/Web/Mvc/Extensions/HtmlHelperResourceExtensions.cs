using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Web.Mvc.Extensions;

public static class HtmlHelperResourceExtensions
{
    private static readonly ConcurrentDictionary<string, string> Cache;
    private static readonly object SyncObj = new object();
    private static ILogger Logger =>
        AutofacDependencyResolver.Current.GetService<ILoggerFactory>().CreateLogger("HtmlHelper");

    static HtmlHelperResourceExtensions()
    {
        Cache = new ConcurrentDictionary<string, string>();
    }

    /// <summary>
    /// Includes a script to the page with versioning.
    /// </summary>
    /// <param name="html">Reference to the HtmlHelper object</param>
    /// <param name="url">URL of the script file</param>
    public static IHtmlString IncludeScript(this HtmlHelper html, string url)
    {
        return html.Raw(
            "<script src=\"" + GetPathWithVersioning(url) + "\" type=\"text/javascript\"></script>"
        );
    }

    /// <summary>
    /// Includes a style to the page with versioning.
    /// </summary>
    /// <param name="html">Reference to the HtmlHelper object</param>
    /// <param name="url">URL of the style file</param>
    public static IHtmlString IncludeStyle(this HtmlHelper html, string url)
    {
        return html.Raw(
            "<link rel=\"stylesheet\" type=\"text/css\" href=\""
                + GetPathWithVersioning(url)
                + "\" />"
        );
    }

    private static string GetPathWithVersioning(string path)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        if (Cache.ContainsKey(path))
        {
            // ReSharper disable once InconsistentlySynchronizedField
            return Cache[path];
        }

        lock (SyncObj)
        {
            if (Cache.ContainsKey(path))
            {
                return Cache[path];
            }

            string result;
            try
            {
                // CDN resource
                if (
                    path.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)
                    || path.StartsWith("//", StringComparison.CurrentCultureIgnoreCase)
                )
                {
                    //Replace "http://" from beginning
                    result = Regex.Replace(path, "^http://", "//", RegexOptions.IgnoreCase);
                }
                else
                {
                    var fullPath = HttpContext.Current.Server.MapPath(path.Replace("/", "\\"));
                    result = File.Exists(fullPath)
                        ? GetPathWithVersioningForPhysicalFile(path, fullPath)
                        : GetPathWithVersioningForEmbeddedFile(path);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Can not find file for: {Path}", path);
                result = path;
            }

            Cache[path] = result;
            return result;
        }
    }

    private static string GetPathWithVersioningForPhysicalFile(string path, string filePath)
    {
        var fileVersion = new FileInfo(filePath).LastWriteTime.Ticks;
        return VirtualPathUtility.ToAbsolute(path) + "?v=" + fileVersion;
    }

    private static string GetPathWithVersioningForEmbeddedFile(string path)
    {
        //Remove "~/" from beginning
        var embeddedResourcePath = path;

        if (embeddedResourcePath.StartsWith("~"))
        {
            embeddedResourcePath = embeddedResourcePath.Substring(1);
        }

        if (embeddedResourcePath.StartsWith("/"))
        {
            embeddedResourcePath = embeddedResourcePath.Substring(1);
        }

        var resource = AutofacDependencyResolver
            .Current.GetService<IVirtualFileProvider>()
            .GetFileInfo(embeddedResourcePath);
        return VirtualPathUtility.ToAbsolute(path) + "?v=" + resource.LastModified.Ticks;
    }
}
