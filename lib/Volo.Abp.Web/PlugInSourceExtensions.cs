using System.Web.Compilation;
using Microsoft.Extensions.Logging;
using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp.Web;

public static class PlugInSourceListExtensions
{
    public static void AddToBuildManager(this IPlugInSource plugInSource, ILogger logger)
    {
        foreach (var plugInType in plugInSource.GetModules())
        {
            try
            {
                var assembly = plugInType.Assembly;
                logger.LogDebug("Adding {assembly.FullName} to BuildManager", assembly.FullName);
                BuildManager.AddReferencedAssembly(assembly);
            }
            catch (Exception ex)
            {
                logger.LogException(ex, LogLevel.Warning);
            }
        }
    }
}
