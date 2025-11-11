namespace Abp.Modularity;

public interface IAbpModule
{
    Task ConfigureServicesAsync(ServiceConfigurationContext context);

    void ConfigureServices(ServiceConfigurationContext context);
}
