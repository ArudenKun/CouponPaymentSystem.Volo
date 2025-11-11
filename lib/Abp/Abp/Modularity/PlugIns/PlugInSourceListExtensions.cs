namespace Abp.Modularity.PlugIns;

public static class PlugInSourceListExtensions
{
    public static void AddFolder(
        this PlugInSourceList list,
        string folder,
        SearchOption searchOption = SearchOption.TopDirectoryOnly
    )
    {
        Check.NotNull(list, nameof(list));

        list.Add(new FolderPlugInSource(folder, searchOption));
    }

    public static void AddTypes(this PlugInSourceList list, params Type[] moduleTypes)
    {
        Check.NotNull(list, nameof(list));

        list.Add(new TypePlugInSource(moduleTypes));
    }

    public static void AddFiles(this PlugInSourceList list, params string[] filePaths)
    {
        Check.NotNull(list, nameof(list));

        list.Add(new FilePlugInSource(filePaths));
    }
}
