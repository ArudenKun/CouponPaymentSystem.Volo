namespace Abp.BlobStoring;

public static class BlobContainerConfigurationExtensions
{
    public static T GetConfiguration<T>(
        this BlobContainerConfiguration containerConfiguration,
        string name
    )
    {
        return (T)containerConfiguration.GetConfiguration(name);
    }

    public static object GetConfiguration(
        this BlobContainerConfiguration containerConfiguration,
        string name
    )
    {
        var value = containerConfiguration.GetConfigurationOrNull(name);
        if (value == null)
        {
            throw new AbpException($"Could not find the configuration value for '{name}'!");
        }

        return value;
    }
}
