using System.Reflection;

namespace System;

public static class AbpAssemblyExtensions
{
    /// <summary>
    /// Gets directory path of given assembly or returns null if can not find.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    public static string? GetDirectoryPathOrNull(this Assembly assembly)
    {
        var location = assembly.Location;
        if (location.IsNullOrEmpty())
        {
            return null;
        }

        var directory = new FileInfo(location).Directory;
        return directory?.FullName;
    }
}
